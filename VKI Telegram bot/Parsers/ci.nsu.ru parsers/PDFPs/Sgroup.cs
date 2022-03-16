using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers.PDFPs
{
    public class Sgroup : PDFP
    {
        public Sgroup(string url = "https://ci.nsu.ru/education/spisok-uchebnykh-grupp/") : base(url)
        {

            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<PDFPEntity> Sgroups = new();

                Sgroups = db.Sgroups.ToList();

                if (Update(Sgroups))
                {
                    db.RemoveRange(db.Sgroups);
                    db.SaveChanges();

                    db.Sgroups.AddRange(list);
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
