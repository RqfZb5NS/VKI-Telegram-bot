using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using VKI_Telegram_bot.DB;
using VKI_Telegram_bot.Parsers.ci_nsu_ru;

namespace VKI_Telegram_bot
{
    public static class Updater
    {
        public static PDFParser iertification = new PDFParser("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
        public static PDFParser sgroup = new PDFParser("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
        public static PDFParser timetable = new("https://ci.nsu.ru/education/schedule/", "timetable");
        public static Schedule schedule = new Schedule("https://ci.nsu.ru/education/raspisanie-zvonkov/", "cschedule");

        private static TelegramBotClient tb;

        public static async Task Start(TelegramBotClient _tb, CancellationToken ctn)
        {
            tb = _tb;
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
                    ctn.WaitHandle.WaitOne(AppSettings.settings.UpdaterAwait);
                }
            }
            catch (Exception ex)
            {
                Log.Warn($"{ex.Message}");
                //Console.WriteLine($"{ex.Message}");
            }
        }
        public static async Task Update(Schedule parser, string messege)
        {
            await parser.UpdateAsync();
            using (VKITGBContext db = new VKITGBContext())
            {
                if (db.ParserDataes.Find(parser.name) != null)
                {
                    if (parser.parserData.JSonData != db.ParserDataes.Find(parser.name)!.JSonData)
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            try
                            {
                                await tb.SendTextMessageAsync(chatId: user.Id, text: messege);
                            }
                            catch (Exception ex)
                            {
                                Log.Warn($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                                //Console.WriteLine($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                            }
                        }
                        db.ParserDataes.Find(parser.name)!.JSonData = parser.parserData.JSonData;
                    }
                }
                else
                {
                    db.ParserDataes.Add(parser.parserData);
                }
                db.SaveChanges();
            }
        }
        public static async Task Update(PDFParser parser, string messege)
        {
            await parser.UpdateAsync();
            using (VKITGBContext db = new VKITGBContext())
            {
                if (db.ParserDataes.Find(parser.name) != null)
                {
                    if (parser.parserData.JSonData != db.ParserDataes.Find(parser.name)!.JSonData)
                    {
                        foreach (var user in db.Users.ToList())
                        {
                            try
                            {
                                await tb.SendTextMessageAsync(chatId: user.Id, text: messege);
                            }
                            catch (Exception ex)
                            {
                                Log.Warn($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                                //Console.WriteLine($"EX: {ex.Message}, USER: Id={user.Id}, Name={user.Name}");
                            }
                        }
                        db.ParserDataes.Find(parser.name)!.JSonData = parser.parserData.JSonData;
                    }
                }
                else
                {
                    db.ParserDataes.Add(parser.parserData);
                }
                db.SaveChanges();
            }
        }

        //private async Task ScheduleUpdate(TelegramBotClient tb, VKITGBContext db)
        //{
        //    await schedule.UpdateAsync();
        //    if (db.ParserDataes.Find(schedule.name) != null)
        //    {
        //        if (!schedule.parserData.Compare(db.ParserDataes.Find(schedule.name)!))
        //        {
        //            foreach (var user in db.Users.ToList())
        //            {
        //                try
        //                {
        //                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание звонков обновилось");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
        //                }
        //            }
        //            db.ParserDataes.Find(schedule.name)!.JSonData = schedule.parserData.JSonData;
        //        }
        //    }
        //    else
        //    {
        //        db.ParserDataes.Add(schedule.parserData);
        //    }
        //}

        //private async Task TimetableUpdate(TelegramBotClient tb, VKITGBContext db)
        //{
        //    await timetable.UpdateAsync();
        //    if (db.ParserDataes.Find(timetable.name) != null)
        //    {
        //        if (!timetable.parserData.Compare(db.ParserDataes.Find(timetable.name)!))
        //        {
        //            foreach (var user in db.Users.ToList())
        //            {
        //                try
        //                {
        //                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Расписание обновилось");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
        //                }
        //            }
        //            db.ParserDataes.Find(timetable.name)!.JSonData = timetable.parserData.JSonData;

        //        }
        //    }
        //    else
        //    {
        //        db.ParserDataes.Add(timetable.parserData);
        //    }
        //}

        //private async Task SgroupUpdate(TelegramBotClient tb, VKITGBContext db)
        //{
        //    await sgroup.UpdateAsync();
        //    if (db.ParserDataes.Find(sgroup.name) != null)
        //    {
        //        if (!sgroup.parserData.Compare(db.ParserDataes.Find(sgroup.name)!))
        //        {
        //            foreach (var user in db.Users.ToList())
        //            {
        //                try
        //                {
        //                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Списки групп обновились");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
        //                }
        //            }
        //            db.ParserDataes.Find(sgroup.name)!.JSonData = sgroup.parserData.JSonData;

        //        }
        //    }
        //    else
        //    {
        //        db.ParserDataes.Add(sgroup.parserData);
        //    }
        //}

        //private async Task IertificationUpdate(TelegramBotClient tb, VKITGBContext db)
        //{
        //    await iertification.UpdateAsync();
        //    if (db.ParserDataes.Find(iertification.name) != null)
        //    {
        //        if (!iertification.parserData.Compare(db.ParserDataes.Find(iertification.name)))
        //        {
        //            foreach (var user in db.Users.ToList())
        //            {
        //                try
        //                {
        //                    await tb.SendTextMessageAsync(chatId: user.Id, text: "Промежуточная аттестация обновилась");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"{ex.Message} {user.Id} {user.Name}");
        //                }

        //            }
        //            db.ParserDataes.Find(iertification.name)!.JSonData = iertification.parserData.JSonData;
        //        }
        //    }
        //    else
        //    {
        //        db.ParserDataes.Add(iertification.parserData);
        //    }
        //}
    }
}
