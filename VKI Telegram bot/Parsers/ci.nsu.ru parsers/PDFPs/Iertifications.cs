using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Iertifications : PDFP
    {
        public Iertifications(string url = "https://ci.nsu.ru/education/raspisanie-ekzamenov/") : base(url)
        {

            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<Iertification> Iertifications = new();

                Iertifications = db.Iertifications.ToList();

                if (Update(ToList(Iertifications)))
                {
                    db.RemoveRange(db.Iertifications);
                    db.SaveChanges();

                    db.Iertifications.AddRange(ToIertification(list));
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<List<string>> ToList(List<Iertification> iertifications)
        {
            List<List<string>> list = new List<List<string>>();
            foreach(Iertification i in iertifications)
            {
                list.Add(new List<string> { i.Link, i.Name });
            }
            return list;
        }

        public List<Iertification> ToIertification(List<List<string>> list)
        {
            List<Iertification> iertifications = new List<Iertification>();
            foreach (List<string> i in list)
            {
                iertifications.Add(new Iertification { Link = i[0], Name = i[1] });
            }
            return iertifications;
        }
    }
}
