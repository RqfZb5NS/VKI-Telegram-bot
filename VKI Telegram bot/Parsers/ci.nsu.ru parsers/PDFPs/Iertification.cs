using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Iertification : PDFP
    {
        public Iertification(string url = "https://ci.nsu.ru/education/raspisanie-ekzamenov/", 
            string name = "iertification") : base(url, name)
        {

        }
    }
}
