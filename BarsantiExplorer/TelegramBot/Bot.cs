using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using System.Security.Cryptography;

namespace BarsantiExplorer.TelegramBot
{
    public class Bot : IHostedService
    {
        private TelegramBotClient BotClient;
        public Bot(string bot_api)
        {
            BotClient = new TelegramBotClient(bot_api);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
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
            Console.WriteLine("API: " + BotClient.LocalBotServer);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Bot stopped");
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
            {
                return;
            }

            if (message.Text is not { } messageText)
            {
                return;
            }
            if (messageText == "test") { 
                await SendTextMessageAsync(botClient, message,"This is a test", cancellationToken);
            }
            else
            {
                await SendTextMessageAsync(botClient, message, "you have said: \n" + messageText, cancellationToken);
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");
        }
        public async Task SendTextMessageAsync(ITelegramBotClient botClient, Message message,string reply,CancellationToken cancellationToken)
        {
            
            await BotClient.SendTextMessageAsync(chatId:message.Chat,text: reply);
        }
        public async void DoWork()
        {
            throw new NotImplementedException();
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

    }
}
