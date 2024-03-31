using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using Telegram.Bot.Types.ReplyMarkups;
using BarsantiExplorer.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using BarsantiExplorer.Enum;

namespace BarsantiExplorer.TelegramBot
{
    public class Bot : BackgroundService
    {
        private TelegramBotClient BotClient;
        private BarsantiDbContext DB;
        private Dictionary<int, Dictionary<long, int>> MessagesStatus = new();
        private string Accept = "Accept";
        private string Reject = "Reject";

        public Bot(string bot_api, string connectionString)
        {
            BotClient = new TelegramBotClient(bot_api);
            var contextOptions =
                new DbContextOptionsBuilder<BarsantiDbContext>().UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString));
            DB = new BarsantiDbContext(contextOptions.Options);
        }

        public async void DoWork(Comment comment)
        {
            var telegramIds = DB.Users
                .Where(el => el.TelegramId != null)
                .Select(el => el.TelegramId!.Value)
                .ToList();

            if (MessagesStatus.ContainsKey(comment.Id))
            {
                MessagesStatus[comment.Id].Clear();
            }
            else
            {
                MessagesStatus.Add(comment.Id, new Dictionary<long, int>());
            }

            var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Accept),
                    InlineKeyboardButton.WithCallbackData(Reject)
                }
            });

            string text = comment.Id + "\n" + comment.Author + " want to add this comment: \n" + comment.Text;
            foreach (var id in telegramIds)
            {
                try
                {
                    var tmp = await BotClient.SendTextMessageAsync(
                        chatId: id,
                        text: text,
                        replyMarkup: inlineKeyboardMarkup
                    );
                    MessagesStatus[comment.Id].Add(id, tmp.MessageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            //callback handling
            if (update.CallbackQuery != null)
            {
                var username = update.CallbackQuery.Message.Chat.FirstName;
                var userId = update.CallbackQuery.Message.Chat.Id;
                var action = update.CallbackQuery.Data;
                var commentId = Convert.ToInt32(update.CallbackQuery.Message.Text.Split("\n")[0]);
                if (action == Accept)
                {
                    DB.Comments.Find(commentId).Status = CommentStatus.Approved;
                }
                else if (action == Reject)
                {
                    DB.Comments.Find(commentId).Status = CommentStatus.Rejected;
                }

                DB.SaveChanges();
                foreach (var dict in MessagesStatus[commentId])
                {
                    if (dict.Key == userId)
                    {
                        await botClient.EditMessageTextAsync(
                            chatId: dict.Key,
                            messageId: dict.Value,
                            text: $"Comment {action}",
                            cancellationToken: cancellationToken
                        );
                    }
                    else
                    {
                        await botClient.EditMessageTextAsync(
                            chatId: dict.Key,
                            messageId: dict.Value,
                            text: $"The comment was {action + " by " + username}",
                            cancellationToken: cancellationToken
                        );
                    }
                }

                MessagesStatus.Remove(commentId);
                return;
            }

            //message handling
            if (update.Message is not { } message)
            {
                return;
            }

            if (message.Text is not { } messageText)
            {
                return;
            }

            if (messageText == "/id")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: $"Your Telegram Token is: {message.Chat.Id}",
                    cancellationToken: cancellationToken
                );
                return;
            }

            await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: $"Write /id to get your Telegram Token",
                cancellationToken: cancellationToken
            );
        }

        //Error handling
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            BotClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Console.WriteLine("Bot started" + BotClient.GetMeAsync());
            return Task.CompletedTask;
        }
    }
}