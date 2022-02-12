using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using static VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.Timetable;

var VKI = new Timetable();
foreach(var i in VKI.doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
{
    Console.WriteLine(i.SelectSingleNode(".//a").GetAttributeValue("href", ""));
    Console.WriteLine(i.SelectSingleNode(".//div[@class='file-name']").InnerText);
}
