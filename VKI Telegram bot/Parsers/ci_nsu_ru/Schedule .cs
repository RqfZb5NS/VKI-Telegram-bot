using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB.Entities;

namespace VKI_Telegram_bot.Parsers.ci_nsu_ru
{
    public class Schedule : Parser
    {
        public ParserData parserData = new();
        public List<List<string>> list = new();
        public InlineKeyboardMarkup? inLine;
        private readonly string _url;
        public string Name { get; set; }
        public Schedule(string url, string name)
        {
            this._url = url;
            Name = name;
            parserData.Name = name;
        }
        public Task UpdateAsync()
        {
            var doc = GetDoc(_url);
            if (doc == null)
            {
                Log.Warn($"Документ is null URl: {_url}, Name: {Name}");
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
                    bts2.Add(InlineKeyboardButton.WithCallbackData(j, $"{Name} {ctr}"));
                    ctr++;
                }
                bts.Add(bts2);
            }
            inLine = new InlineKeyboardMarkup(bts);
            return Task.CompletedTask;
        }
    }
}
