using VKI_Telegram_bot.Parsers;
using static VKI_Telegram_bot.Parsers.GetHtml;

var VKI = new GetHtml("https://ci.nsu.ru/education/schedule/");
Console.WriteLine(VKI.html);