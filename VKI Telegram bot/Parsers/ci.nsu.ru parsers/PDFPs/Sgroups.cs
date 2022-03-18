using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Sgroups : PDFP
    {
        public Sgroups(string url = "https://ci.nsu.ru/education/spisok-uchebnykh-grupp/") : base(url)
        {

            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<Sgroup> Sgroups = new();

                Sgroups = db.Sgroups.ToList();

                if (Update(ToList(Sgroups)))
                {
                    db.RemoveRange(db.Sgroups);
                    db.SaveChanges();

                    db.Sgroups.AddRange(ToSgroup(list));
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public List<List<string>> ToList(List<Sgroup> sgroups)
        {
            List<List<string>> list = new List<List<string>>();
            foreach (Sgroup i in sgroups)
            {
                list.Add(new List<string> { i.Link, i.Name });
            }
            return list;
        }

        public List<Sgroup> ToSgroup(List<List<string>> list)
        {
            List<Sgroup> sgroups = new List<Sgroup>();
            foreach (List<string> i in list)
            {
                sgroups.Add(new Sgroup { Link = i[0], Name = i[1] });
            }
            return sgroups;
        }
    }
}
