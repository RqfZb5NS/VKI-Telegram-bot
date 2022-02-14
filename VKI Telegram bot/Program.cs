using Microsoft.Data.Sqlite;
using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using VKI_Telegram_bot.DB;

var timetable = new PDFP("https://ci.nsu.ru/education/schedule/");
var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
var iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/");
var cschedule = new CSchedule();
foreach (var i in timetable.list)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
Console.WriteLine("\n");
foreach (var i in sgroup.list)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
Console.WriteLine("\n");
foreach (var i in cschedule.list)
{
    foreach (var j in i)
    {
        Console.Write($"{j} ");
    }
    Console.WriteLine();
}
Console.WriteLine("\n");
foreach (var i in iertification.list)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
//var usertable = new UsersTable();
//usertable.DelUser(2);
//foreach (var i in usertable.GetUserList())
//{
//    Console.WriteLine($"{i[0]} {i[1]}");
//}
//Console.Read();