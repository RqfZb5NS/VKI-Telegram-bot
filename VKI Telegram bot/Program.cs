using VKI_Telegram_bot;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using VKI_Telegram_bot.Parsers.ci_nsu_ru;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot;
public static class Program
{


    public static CancellationTokenSource cts = new CancellationTokenSource();
    public static TelegramBotClient tb = new TelegramBotClient(AppSettings.settings.BotApiKey);

    public static async Task Main()
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCancerHandler);

        Telegram.Bot.Types.User me = await tb.GetMeAsync();
        Console.Title = me.Username ?? "VKI Telegram bot";
        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };

        _ = Task.Run(() => Updater.Start(tb, cts.Token));
        tb.StartReceiving(Handlers.HandleUpdateAsync,
                           Handlers.HandleErrorAsync,
                           receiverOptions,
                           cts.Token);
        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();
        cts.Cancel();
        cts.Dispose();
    }
    private static void ConsoleCancerHandler(object sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("закрытие приложения");
        cts.Cancel();
        cts.Dispose();
    }
}
