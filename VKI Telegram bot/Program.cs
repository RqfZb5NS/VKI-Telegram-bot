using VKI_Telegram_bot.DB;
using Microsoft.Extensions.Configuration;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using System.Text.Json;
using VKI_Telegram_bot.Telegram;
using VKI_Telegram_bot;

//Console.WriteLine(AppSettings.settings.BotApiKey);

Updater parser = new Updater();
Bot.Start();
//foreach (var i in parser.iertification.list)
//{
//    Console.WriteLine($"{ i[0]}\n{ i[1] }");
//}

//using (VKITGBContext db = new VKITGBContext())
//{
//    foreach (var i in JsonSerializer.Deserialize<List<List<string>>>(db.Dates.Find("timetable").JSonData))
//    {
//        Console.WriteLine($"{ i[0]}\n{ i[1] }");
//    }
//}