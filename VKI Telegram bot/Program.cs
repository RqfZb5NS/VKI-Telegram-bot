using Microsoft.Data.Sqlite;
using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.DB;
using Microsoft.Extensions.Configuration;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using System.Text.Json;

var builder = new ConfigurationBuilder(); // установка пути к текущему каталогу
builder.SetBasePath(Directory.GetCurrentDirectory()); // получаем конфигурацию из файла appsettings.json
builder.AddJsonFile("appsettings.json"); // создаем конфигурацию
var config = builder.Build(); // получаем строку подключения
                              //Console.WriteLine(config.GetSection("TG").GetSection("Key").Value);

var iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
var timetable = ("https://ci.nsu.ru/education/schedule/", "timetable");
var cschedule = new CSchedule();

using (VKITGBContext db = new VKITGBContext())
{
    foreach (var i in JsonSerializer.Deserialize<List<List<string>>>(db.Dates.Find("timetable").JSonData))
    {
        Console.WriteLine($"{ i[0]}\n{ i[1] }");
    }
}