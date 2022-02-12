using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using static VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFP;

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
foreach(var i in cschedule.doc.DocumentNode.SelectSingleNode(".//table[@class='table']").SelectNodes(".//tr"))
{
    try 
    {
        foreach (var j in i.SelectNodes(".//th"))
        {
            Console.WriteLine(j.GetAttributeValue("p", ""));
        }
    }
    catch (Exception ex)
    {
        foreach (var j in i.SelectNodes(".//td"))
        {
            Console.WriteLine(j.GetAttributeValue("p", ""));
        }
    }
}