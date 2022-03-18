using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Timetables : PDFP
    {
        public Timetables(string url = "https://ci.nsu.ru/education/schedule/") : base(url)
        {
            
            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<Timetable> Timetables = new();

                Timetables = db.Timetables.ToList();

                if (Update(ToList(Timetables)))
                {
                    db.RemoveRange(db.Timetables);
                    db.SaveChanges();

                    db.Timetables.AddRange(ToTimetable(list));
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public List<List<string>> ToList(List<Timetable> timetables)
        {
            List<List<string>> list = new List<List<string>>();
            foreach (Timetable i in timetables)
            {
                list.Add(new List<string> { i.Link, i.Name });
            }
            return list;
        }

        public List<Timetable> ToTimetable(List<List<string>> list)
        {
            List<Timetable> timetables = new List<Timetable>();
            foreach (List<string> i in list)
            {
                timetables.Add(new Timetable { Link = i[0], Name = i[1] });
            }
            return timetables;
        }
    }
}
