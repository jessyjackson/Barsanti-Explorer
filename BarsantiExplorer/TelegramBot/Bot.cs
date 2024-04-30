using BarsantiExplorer.Enum;
using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BarsantiExplorer.TelegramBot
{
    public class Bot : BackgroundService
    {
        private readonly TelegramBotClient BotClient;
        private readonly BarsantiDbContext DB;
        private Dictionary<int, Dictionary<long, int>> MessagesStatus;
        private readonly string Accept = "Accepted";
        private readonly string Reject = "Rejected";

        public Bot(string bot_api, string connectionString)
        {
            BotClient = new TelegramBotClient(bot_api);
            var contextOptions =
                new DbContextOptionsBuilder<BarsantiDbContext>().UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString));
            DB = new BarsantiDbContext(contextOptions.Options);
            MessagesStatus = new Dictionary<int, Dictionary<long, int>>();
        }

        public async void SendNewCommentToAdmin(Comment comment)
        {
            var telegramIds = DB.Users
                .Where(el => el.TelegramId != null)
                .Select(el => el.TelegramId!.Value)
                .ToList();
            var trip = DB.Trips.Find(comment.TripId);

            if (MessagesStatus.TryGetValue(comment.Id, out Dictionary<long, int>? value))
            {
                value.Clear();
            }
            else
            {
                MessagesStatus.Add(comment.Id, []);
            }

            var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Accept),
                    InlineKeyboardButton.WithCallbackData(Reject)
                }
            });

            string text = comment.Id + "\n" + "Under: " + trip?.Title + "\n" + "Rating: " + comment.Rating + "\n" + "Author: " + comment.Author + "\n" + "Comment: " + comment.Text;
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
                catch (Exception exception)
                {
                    var ErrorMessage = exception switch
                    {
                        ApiRequestException apiRequestException
                            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                        _ => exception.ToString()
                    };
                    Console.WriteLine(ErrorMessage);
                }
            }
        }
        private async Task HandleCallBack(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var username = update.CallbackQuery.Message.Chat.FirstName;
            var userId = update.CallbackQuery.Message.Chat.Id;
            var action = update.CallbackQuery.Data;
            var commentId = Convert.ToInt32(update.CallbackQuery.Message.Text.Split("\n")[0]);
            if (action == Accept)
            {
                DB.Comments.Find(commentId)!.Status = CommentStatus.Approved;
            }
            else if (action == Reject)
            {
                DB.Comments.Find(commentId)!.Status = CommentStatus.Rejected;
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
        private static async Task HandleMessage(Message message, string messageText, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            if (messageText == "/id" || messageText == "/Id" || messageText == "/ID")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: $"Your Telegram Token is: {message.Chat.Id}",
                    cancellationToken: cancellationToken
                );
                return;
            }
            if (messageText == "/start" || messageText == "/help" || messageText == "/Start" || messageText == "/Help")
            {
                await botClient.SendTextMessageAsync(
                      chatId: message.Chat,
                      text: $"Welcome to our Barsanti Explorer Bot!\r\n\r\n" +
                      $"Our Comment Moderator Bot helps manage user comments by allowing you to accept or deny comments submitted by users. Whether you want to maintain a positive community atmosphere or ensure that only appropriate comments are displayed, our bot has got you covered.\r\n\r\n" +
                      $"Key Features:\r\n\r\n" +
                      $"1) Accept or Deny Comments: With just a few clicks, you can review comments submitted by users and decide whether to accept or deny them for display.\r\n" +
                      $"2) Real-Time Notifications: Receive instant notifications whenever a new comment is submitted, allowing you to promptly review and take action.\r\n" +
                      $"3) Secure and Reliable: Rest assured that your data and interactions with the bot are kept secure and confidential.\r\n\r\n" +
                      $"How to Use:\r\n\r\n" +
                      $"1) Send /id to get your telegram id\r\n" +
                      $"2) Put It on the website in the sync telegram section\r\n" +
                      $"3) Accept or deny by simple press the buttons that will appear under the message",
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
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            //callback handling
            if (update.CallbackQuery != null)
            {
                await HandleCallBack(botClient, update, cancellationToken);
                return;
            }
            //message handling
            if (update.Message != null && update.Message.Text != null)
            {
                await HandleMessage(update.Message, update.Message.Text, botClient, cancellationToken);
                return;
            }

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

            Console.WriteLine("Bot: " + BotClient.GetMyNameAsync(cancellationToken: stoppingToken).Result.Name);
            return Task.CompletedTask;
        }
    }
}