using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace VKI_Telegram_bot;
public static class Program
{
    public static CancellationTokenSource cts = new();
    public static TelegramBotClient? tb = new(Environment.GetEnvironmentVariable("TELEGRAM_TOKEN"));

    public static async Task Main()
    {
        Console.CancelKeyPress += (sender, args) =>
        {
            Log.Info("Закрытие приложения");
            cts.Cancel();
            cts.Dispose();
        };
        Telegram.Bot.Types.User me = await tb.GetMeAsync();
        Console.Title = me.Username ?? "VKI Telegram bot";
        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };

        _ = Task.Run(() => Updater.Start(tb, cts.Token));
        tb.StartReceiving(Handlers.HandleUpdateAsync,
                           Handlers.HandleErrorAsync,
                           receiverOptions,
                           cts.Token);
        Log.Info($"Start listening for @{ me.Username}");
        Console.ReadLine();
        cts.Cancel();
        cts.Dispose();
        Log.Info("Закрытие приложения");
    }
}
