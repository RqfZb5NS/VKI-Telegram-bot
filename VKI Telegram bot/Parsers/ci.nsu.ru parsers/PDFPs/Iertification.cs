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
        public Iertification(string url = "https://ci.nsu.ru/education/raspisanie-ekzamenov/") : base(url)
        {

            //_ = Update();
        }
        public bool Update()
        {
            using (VKITGBContext db = new VKITGBContext())
            {
                List<PDFPEntity> Iertifications = new();

                Iertifications = db.Iertifications.ToList();

                if (Update(Iertifications))
                {
                    db.RemoveRange(db.Iertifications);
                    db.SaveChanges();

                    db.Iertifications.AddRange(list);
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
