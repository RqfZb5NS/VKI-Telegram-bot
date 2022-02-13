using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;

var timetable = new PDFP("https://ci.nsu.ru/education/schedule/");
var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
var cschedule = new CSchedule();
foreach (var i in timetable.returned)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
Console.WriteLine("\n");
foreach (var i in sgroup.returned)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
Console.WriteLine("\n");
foreach(var i in cschedule.returned)
{
    foreach (var j in i)
    {
        Console.Write($"{j} ");
    }
    Console.WriteLine();
}