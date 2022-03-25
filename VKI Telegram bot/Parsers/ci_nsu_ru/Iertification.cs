using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers.ci_nsu_ru
{
    public class Iertification : PDFParser
    {
        public Iertification() : base("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification")
        {

        }
    }
}
