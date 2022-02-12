using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using static VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFP;

var timetable = new PDFP("https://ci.nsu.ru/education/schedule/");
var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
foreach (var i in timetable.returned)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
foreach (var i in sgroup.returned)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
}
