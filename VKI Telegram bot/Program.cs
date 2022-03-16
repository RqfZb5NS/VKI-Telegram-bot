using Microsoft.Data.Sqlite;
using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using VKI_Telegram_bot.DB;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder(); // установка пути к текущему каталогу
builder.SetBasePath(Directory.GetCurrentDirectory()); // получаем конфигурацию из файла appsettings.json
builder.AddJsonFile("appsettings.json"); // создаем конфигурацию
var config = builder.Build(); // получаем строку подключения
//Console.WriteLine(config.GetSection("TG").GetSection("Key").Value);

var timetable = new PDFP("https://ci.nsu.ru/education/schedule/");
//var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
//PDFP iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "Iertifications");
//var cschedule = new CSchedule();
foreach (var i in timetable.list)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
//timetable.list.Add(new string[] { "ss", "ss" });
//Console.WriteLine("\n");
//foreach (var i in sgroup.list)
//{
//    Console.WriteLine($"{ i[0] }\n{ i[1] }");
//}
//Console.WriteLine("\n");
//foreach (var i in cschedule.list)
//{
//    foreach (var j in i)
//    {
//        Console.Write($"{j} ");
//    }
//    Console.WriteLine();
//}
//Console.WriteLine("\n");
//foreach (var i in iertification.list)
//{
//    Console.WriteLine($"{ i[0] }\n{ i[1] }");
//}
//iertificationstable.Clear();
//var usertable = new UsersTable();
//usertable.DelUser(2);
//foreach (var i in usertable.GetUserList())
//{
//    Console.WriteLine($"{i[0]} {i[1]}");
//}
//Console.Read();