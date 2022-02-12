using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class Timetable : Parser
    {
        public Timetable(string url = "https://ci.nsu.ru/education/schedule/") : base(url)
        {
            var pdf = doc.DocumentNode.SelectNodes(".//div[@class='file-div']");
            //foreach (var filediv in pdf)
            //{

            //}
        }
    }
}
