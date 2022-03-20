using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace VKI_Telegram_bot.Telegram
{
    public static class Bot
    {
        public static TelegramBotClient? tb;

        public static async Task Start()
        {
            tb = new TelegramBotClient(AppSettings.settings.BotApiKey);

            User me = await tb.GetMeAsync();
            Console.Title = me.Username ?? "My awesome Bot";
            using var cts = new CancellationTokenSource();

            // StartReceiving не блокирует вызывающий поток. Прием осуществляется на ThreadPool.
            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            tb.StartReceiving(Handlers.HandleUpdateAsync,
                               Handlers.HandleErrorAsync,
                               receiverOptions,
                               cts.Token);
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            // Отправить cancellation request, чтобы остановить бота
            cts.Cancel();
        }
    }
}
