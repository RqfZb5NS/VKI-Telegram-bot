using VKI_Telegram_bot;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using VKI_Telegram_bot.Parsers.ci_nsu_ru;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot;
public static class Program
{
    public static PDFParser iertification = new PDFParser("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
    public static PDFParser sgroup = new PDFParser("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
    public static PDFParser timetable = new PDFParser("https://ci.nsu.ru/education/schedule/", "timetable");
    public static Schedule schedule = new Schedule("https://ci.nsu.ru/education/raspisanie-zvonkov/", "cschedule");
    public static Thread uptadeThread = new Thread(new ParameterizedThreadStart(StartUpdate));


    public static TelegramBotClient tb;

    public static async Task Main()
    {
        tb = new TelegramBotClient(AppSettings.settings.BotApiKey);
        CancellationTokenSource cts = new CancellationTokenSource();
        uptadeThread.Start(cts);

        Telegram.Bot.Types.User me = await tb.GetMeAsync();
        Console.Title = me.Username ?? "VKI Telegram bot";
        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
        tb.StartReceiving(Handlers.HandleUpdateAsync,
                           Handlers.HandleErrorAsync,
                           receiverOptions,
                           cts.Token);
        Console.WriteLine($"Start listening for @{me.Username}");
        while (!cts.IsCancellationRequested)
        {
            string? command = Console.ReadLine();
            if(command != null)
            {
                switch (command)
                {
                    case "exit":
                        {
                            Console.WriteLine("exit");
                            cts.Cancel();
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
    public static async void StartUpdate(object obj)
    {
        CancellationTokenSource ctn = obj as CancellationTokenSource;

        try
        {
            while (!ctn.IsCancellationRequested)
            {
                Console.WriteLine("Обновление парсера");
                using (VKITGBContext db = new VKITGBContext())
                {
                    await iertification.UpdateAsync();
                    if (db.ParserDataes.Find(iertification.name) != null)
                    {
                        if (!iertification.parserData.Compare(db.ParserDataes.Find(iertification.name)!))
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                try
                                {
                                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Промежуточная аттестация обновилась");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                
                            }
                            db.ParserDataes.Find(iertification.name)!.JSonData = iertification.parserData.JSonData;
                        }
                    }
                    else
                    {
                        db.ParserDataes.Add(iertification.parserData);
                    }

                    await sgroup.UpdateAsync();
                    if (db.ParserDataes.Find(sgroup.name) != null)
                    {
                        if (!sgroup.parserData.Compare(db.ParserDataes.Find(sgroup.name)!))
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                try
                                {
                                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Списки групп обновились");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
                                }
                            }
                            db.ParserDataes.Find(sgroup.name)!.JSonData = sgroup.parserData.JSonData;

                        }
                    }
                    else
                    {
                        db.ParserDataes.Add(sgroup.parserData);
                    }

                    await timetable.UpdateAsync();
                    if (db.ParserDataes.Find(timetable.name) != null)
                    {
                        if (!timetable.parserData.Compare(db.ParserDataes.Find(timetable.name)!))
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                try
                                {
                                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание обновилось");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
                                }
                            }
                            db.ParserDataes.Find(timetable.name)!.JSonData = timetable.parserData.JSonData;

                        }
                    }
                    else
                    {
                        db.ParserDataes.Add(timetable.parserData);
                    }

                    await schedule.UpdateAsync();
                    if (db.ParserDataes.Find(schedule.name) != null)
                    {
                        if (!schedule.parserData.Compare(db.ParserDataes.Find(schedule.name)!))
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                try
                                {
                                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание звонков обновилось");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
                                }
                            }
                            db.ParserDataes.Find(schedule.name)!.JSonData = schedule.parserData.JSonData;
                        }
                    }
                    else
                    {
                        db.ParserDataes.Add(schedule.parserData);
                    }
                    db.SaveChanges();
                }
                ctn.Token.WaitHandle.WaitOne(AppSettings.settings.UpdaterAwait);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}
