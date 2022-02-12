using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using static VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.Timetable;

var VKI = new Timetable();
foreach (var i in VKI.returned)
{
    Console.WriteLine($"{ i[0] }\n{ i[1] }");
    Console.WriteLine();
}
