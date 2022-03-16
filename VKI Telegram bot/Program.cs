using Microsoft.Data.Sqlite;
using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.DB;
using Microsoft.Extensions.Configuration;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs;

var builder = new ConfigurationBuilder(); // установка пути к текущему каталогу
builder.SetBasePath(Directory.GetCurrentDirectory()); // получаем конфигурацию из файла appsettings.json
builder.AddJsonFile("appsettings.json"); // создаем конфигурацию
var config = builder.Build(); // получаем строку подключения
//Console.WriteLine(config.GetSection("TG").GetSection("Key").Value);

var timetable = new Timetable();
Console.WriteLine(timetable.Update());
foreach (var i in timetable.list)
{
    Console.WriteLine($"{ i.Name}\n{ i.Link }");
}

//var cschedule = new CSchedule();