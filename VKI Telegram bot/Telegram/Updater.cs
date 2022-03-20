using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace VKI_Telegram_bot.Telegram
{
    public class Updater
    {
        
        public static PDFP iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/", "iertification");
        public static PDFP sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/", "sgroup");
        public static PDFP timetable = new PDFP("https://ci.nsu.ru/education/schedule/", "timetable");
        public static CSchedule cschedule = new CSchedule();
        public static Thread thread = new Thread(new ThreadStart(Parsing));
        public Updater()
        {
            thread.Start();
        }
        public static async void Parsing()
        {
            while (true)
            {
                Thread.Sleep(10800000);
                if (iertification.Update())
                {
                    
                }
                if (sgroup.Update())
                {

                }
                if (timetable.Update())
                {

                }
                if (cschedule.Update())
                {

                }
            }
        }
    }
}