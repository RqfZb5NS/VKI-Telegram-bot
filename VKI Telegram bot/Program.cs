using Telegram.Bot.Extensions.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;

namespace VKI_Telegram_bot.Telegram
{
    public static class Program
    {
        public static PDFP iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
        public static PDFP sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
        public static PDFP timetable = new PDFP("https://ci.nsu.ru/education/schedule/", "timetable");
        public static CSchedule cschedule = new CSchedule();
        public static Thread thread = new Thread(new ThreadStart(Updater));

        public static TelegramBotClient? tb;

        public static async Task Main()
        {

            thread.Start();
            tb = new TelegramBotClient(AppSettings.settings.BotApiKey);
            
            User me = await tb.GetMeAsync();
            Console.Title = me.Username ?? "VKI Telegram bot";
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
        public static void Updater()
        {
            while (true)
            {
                //Thread.Sleep(60000); // 10800000
  
                Console.WriteLine("Updating...");
                try
                {
                    if (iertification.Update())
                    {
                        using (DB.VKITGBContext db = new DB.VKITGBContext())
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                //var usr = tb.GetChatMemberAsync();
                                _ = tb.SendTextMessageAsync(chatId: user.Id, text: "Промежуточная аттестация обновилась");
                            }
                        }
                    }
                    if (sgroup.Update())
                    {
                        using (DB.VKITGBContext db = new DB.VKITGBContext())
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                tb.SendTextMessageAsync(chatId: user.Id, text: "Списки групп обновились");
                            }
                        }
                    }
                    if (timetable.Update())
                    {
                        using (DB.VKITGBContext db = new DB.VKITGBContext())
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание обновилось");
                            }
                        }
                    }
                    if (cschedule.Update())
                    {
                        using (DB.VKITGBContext db = new DB.VKITGBContext())
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание звонков обновилось");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(10800000);
            }
        }
    }
}