using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace BarsantiExplorer.TelegramBot
{
    public class Bot : BackgroundService
    {
        private TelegramBotClient BotClient;
        public Bot(string bot_api)
        {
            BotClient = new TelegramBotClient(bot_api);
        }
        public async void DoWork(BarsantiDbContext db,Comment comment)
        {
            var users = db.Users
                .Select(el => el.TelegramId)
                .ToList();

            var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Accept"),
                    InlineKeyboardButton.WithCallbackData("Deny")
                }
            });
            foreach (var user in users)
            {
               await BotClient.SendTextMessageAsync(
                    chatId: user,
                    text: "Hello! I am a bot that can echo back your messages. Just type anything and I will send it back to you!",
                    replyMarkup: inlineKeyboardMarkup
                );
            }
        }
        public async void OnCallBack(object sender,CallbackQuery e)
        {
            var message = e.Message;
            var callbackData = e.Data;
            Console.WriteLine($"Received callback data {callbackData} in chat {message.Chat.Id}.");
            
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //callback handling
            if (update.CallbackQuery != null)
            {
                Console.WriteLine("Received callback data " + update.CallbackQuery.Data);
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

            if (messageText == "/help")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: "I am a bot that can echo back your messages. Just type anything and I will send it back to you!",
                    cancellationToken: cancellationToken
                    );
            }
            else if(messageText == "/id")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: message.Chat.Id.ToString(),
                    cancellationToken: cancellationToken
                );
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");
        }
        //Error handling
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
