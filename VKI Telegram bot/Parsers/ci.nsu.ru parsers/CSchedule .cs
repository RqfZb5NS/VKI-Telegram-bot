using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class CSchedule : Parser
    {
        public List<List<string>> list = new List<List<string>>();
        public InlineKeyboardMarkup InLine;
        public string name = "";
        public CSchedule(string url = "https://ci.nsu.ru/education/raspisanie-zvonkov/", 
            string _name = "cschedule") : base(url)
        {
            name = _name;
            //_ = Update();
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
            int ctr = 0;
            foreach (var i in doc.DocumentNode.SelectSingleNode(".//table[@class='table']").SelectNodes(".//tr"))
            {
                list.Add(new List<string>());
                foreach (var j in i.SelectNodes(".//p"))
                {
                    list[ctr].Add(j.InnerText.Trim());
                }
                ctr++;
            }
            list = list
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();
            if (list.Count == list2.Count && list[0].Count == list2[0].Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        if (list[i][j] != list2[i][j])
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
                            GetInLine();
                            return true;
                        }
                    }
                }
                GetInLine();
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
                GetInLine();
                return true;
            }
        }
        public InlineKeyboardMarkup GetInLine()
        {
            List<List<InlineKeyboardButton>> bts = new();
            int ctr = 0;
            foreach (var i in list)
            {
                List<InlineKeyboardButton> bts2 = new();
                foreach (var j in i)
                {
                    bts2.Add(InlineKeyboardButton.WithCallbackData(j, $"{name} {ctr}"));
                    ctr++;
                }
                bts.Add(bts2);
            }
            InLine = new InlineKeyboardMarkup(bts);
            return new(bts);
        }

    }
}
