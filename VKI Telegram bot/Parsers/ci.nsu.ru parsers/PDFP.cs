using VKI_Telegram_bot.DB;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<List<string>> list = new();
        public string name = "";
        public PDFP(string url, string _name) : base(url)
        {
            name = _name;
            _ = Update();
        }
        public bool Update()
        {
            List<List<string>>? list2 = new();
            using (VKITGBContext db = new VKITGBContext())
            {
                if (db.Dates.Find(name) != null)
                {
                    list2 = JsonSerializer.Deserialize<List<List<string>>>(db.Dates.Find(name).JSonData);
                }
                else
                {
                    list2 = new List<List<string>>();
                }

            }
            list.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string link = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new List<string> { name, link });

            }
            //Console.WriteLine(JsonSerializer.Serialize(list));
            if (list.Count == list2.Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i][0] != list2[i][0] || list[i][1] != list2[i][1])
                    {

                        using (VKITGBContext db = new VKITGBContext())
                        {
                            if (db.Dates.Find(name) != null)
                            {
                                db.Dates.Find(name).JSonData = JsonSerializer.Serialize(list);
                            }
                            else
                            {
                                db.Dates.Add(new Data { JSonData = JsonSerializer.Serialize(list), Name = name });
                            }
                            db.SaveChanges();
                        }
                        return true;
                    }
                }
                return false;
            }
            else
            {
                using (VKITGBContext db = new VKITGBContext())
                {
                    if (db.Dates.Find(name) != null)
                    {
                        db.Dates.Find(name).JSonData = JsonSerializer.Serialize(list);
                    }
                    else
                    {
                        db.Dates.Add(new Data { JSonData = JsonSerializer.Serialize(list), Name = name });
                    }
                    db.SaveChanges();
                }
                return true;
            }
        }
    }
}
