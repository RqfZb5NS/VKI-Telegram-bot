using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci_nsu_ru
{
    public class Schedule : Parser
    {
        public ParserData parserData = new();
        public List<List<string>> list = new();
        public InlineKeyboardMarkup? InLine;
        public string name { get; set; }
        public Schedule(string url, string _name) : base(url)
        {
            name = _name;
            parserData.Name = _name;
        }
        public Task UpdateAsync()
        {
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
            parserData.SetDataFromList(list);
            List<List<InlineKeyboardButton>> bts = new();
            ctr = 0;
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
            return Task.CompletedTask;
        }
        //public Task UpdateInLineAsync() => Task.Run(() =>
        //{
        //    List<List<InlineKeyboardButton>> bts = new();
        //    int ctr = 0;
        //    foreach (var i in list)
        //    {
        //        List<InlineKeyboardButton> bts2 = new();
        //        foreach (var j in i)
        //        {
        //            bts2.Add(InlineKeyboardButton.WithCallbackData(j, $"{name} {ctr}"));
        //            ctr++;
        //        }
        //        bts.Add(bts2);
        //    }
        //    InLine = new InlineKeyboardMarkup(bts);
        //});
    }
}
