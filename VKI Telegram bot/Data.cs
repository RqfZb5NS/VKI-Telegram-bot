using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;

namespace VKI_Telegram_bot
{

    internal class Data
    {
        public PDFP Timetable = new("https://ci.nsu.ru/education/schedule/");
        public PDFP Sgroup = new("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
        public PDFP Iertification = new("https://ci.nsu.ru/education/raspisanie-ekzamenov/");
        public CSchedule Cschedule = new();

    }
}
