﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Timetable : PDFP
    {
        public Timetable(string url = "https://ci.nsu.ru/education/schedule/") : base(url)
        {
            
            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<PDFPEntity> Timetables = new();

                Timetables = db.Timetables.ToList();

                if (Update(Timetables))
                {
                    db.RemoveRange(db.Timetables);
                    db.SaveChanges();

                    db.Timetables.AddRange(list);
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}