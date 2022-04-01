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
        private readonly string url;
        public string name { get; set; }
        public Schedule(string _url, string _name)
        {
            url = _url;
            name = _name;
            parserData.Name = _name;
        }
        public Task UpdateAsync()
        {
            var doc = GetDoc(url);
            if (doc == null)
            {
                Log.Warn($"Документ is null URl: {url}, Name: {name}");
                return Task.CompletedTask;
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
    }
}
