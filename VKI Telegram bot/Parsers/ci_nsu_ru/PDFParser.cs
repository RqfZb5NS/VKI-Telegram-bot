using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB.Entities;

namespace VKI_Telegram_bot.Parsers.ci_nsu_ru
{
    public class PdfParser : Parser
    {
        public ParserData parserData = new();
        public List<List<string>> list = new();
        public InlineKeyboardMarkup? inLine;
        public string Name { get; set; }
        private readonly string _url;
        public PdfParser(string url, string name)
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
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string link = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new List<string> { name, link });
            }
            parserData.SetDataFromList(list);
            List<InlineKeyboardButton[]> bts = new();
            int ctr = 0;
            foreach (var i in list)
            {
                bts.Add(new[] { InlineKeyboardButton.WithCallbackData(i[0], $"{Name} {ctr}") });
                ctr++;
            }
            inLine = new InlineKeyboardMarkup(bts);
            return Task.CompletedTask;
        }
    }
}
