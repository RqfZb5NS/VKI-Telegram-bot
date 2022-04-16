using Telegram.Bot;
using VKI_Telegram_bot.DB;
using VKI_Telegram_bot.Parsers.ci_nsu_ru;

namespace VKI_Telegram_bot
{
    public static class Updater
    {
        public static PdfParser iertification = new("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
        public static PdfParser sgroup = new("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
        public static PdfParser timetable = new("https://ci.nsu.ru/education/schedule/", "timetable");
        public static Schedule schedule = new("https://ci.nsu.ru/education/raspisanie-zvonkov/", "cschedule");

        private static TelegramBotClient? _tb;

        public static async Task Start(TelegramBotClient? tb, CancellationToken ctn)
        {
            Updater._tb = tb;
            try
            {
                while (!ctn.IsCancellationRequested)
                {
                    Log.Info("Обновление парсеров");
                    //Console.WriteLine("Обновление парсеров");
                    await Update(iertification, "Промежуточная аттестация обновилась");
                    await Update(sgroup, "Списки групп обновились");
                    await Update(timetable, "Расписание обновилось");
                    await Update(schedule, "Расписание звонков обновилось");
                    Log.Info("Парсеры обновлены");
                    //Console.WriteLine("Парсеры обновлены");
                    ctn.WaitHandle.WaitOne(AppSettings.Settings.UpdaterAwait);
                }
            }
            catch (Exception ex)
            {
                Log.Warn($"{ex.Message}");
                //Console.WriteLine($"{ex.Message}");
            }
        }
        public static async Task Update(Schedule parser, string msg)
        {
            await parser.UpdateAsync();
            await using var db = new DataBaseContext();
            if (await db.ParserDataes.FindAsync(parser.Name) != null)
            {
                if (parser.parserData.JSonData != (await db.ParserDataes.FindAsync(parser.Name))!.JSonData)
                {
                    foreach (var user in db.Users.ToList())
                    {
                        try
                        {
                            await _tb!.SendTextMessageAsync(chatId: user.Id, text: msg);
                        }
                        //catch()
                        //{

                        //}
                        catch (Exception ex)
                        {
                            Log.Warn($"EX: {ex}, USER: Id={user.Id}, Name={user.Name}");
                            //Console.WriteLine($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                        }
                    }
                    (await db.ParserDataes.FindAsync(parser.Name))!.JSonData = parser.parserData.JSonData;
                }
            }
            else
            {
                db.ParserDataes.Add(parser.parserData);
            }
            await db.SaveChangesAsync();
        }
        public static async Task Update(PdfParser parser, string msg)
        {
            await parser.UpdateAsync();
            await using DataBaseContext db = new();
            if (await db.ParserDataes.FindAsync(parser.Name) != null)
            {
                if (parser.parserData.JSonData != (await db.ParserDataes.FindAsync(parser.Name))!.JSonData)
                {
                    foreach (var user in db.Users.ToList())
                    {
                        try
                        {
                            await _tb!.SendTextMessageAsync(chatId: user.Id, text: msg);
                        }
                        catch (Telegram.Bot.Exceptions.RequestException ex)
                        {
                            if (ex.Message == "Forbidden: bot was blocked by the user")
                            {
                                Log.Info($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                                db.Users.Remove(user);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"USER: Id={user.Id}, Name={user.Name}", ex);
                        }
                    }
                    (await db.ParserDataes.FindAsync(parser.Name))!.JSonData = parser.parserData.JSonData;
                }
            }
            else
            {
                db.ParserDataes.Add(parser.parserData);
            }
            await db.SaveChangesAsync();
        }
    }
}
