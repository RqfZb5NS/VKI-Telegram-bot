using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Timetable : PDFP
    {
        public Timetable(string url = "https://ci.nsu.ru/education/schedule/", 
            string name = "timetable") : base(url, name)
        {
            
        }
    }
}
