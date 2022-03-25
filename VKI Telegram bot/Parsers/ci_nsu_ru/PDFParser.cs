using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci_nsu_ru
{
    public class PDFParser : Parser
    {
        public ParserData parserData = new();
        public List<List<string>> list = new();
        public InlineKeyboardMarkup? inLine;
        public string name { get; set; }
        public PDFParser(string url, string _name) : base(url)
        {
            name = _name;
            parserData.Name = _name;
        }

        public async Task UpdateAsync()
        {
            list.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string link = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new List<string> { name, link });
            }
            parserData.SetDataFromList(list);
            await UpdateInLineAsync();
        }
        public Task UpdateInLineAsync() => Task.Run(() =>
        {
            List<InlineKeyboardButton[]> bts = new();
            int ctr = 0;
            foreach (var i in list)
            {
                bts.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(i[0], $"{name} {ctr}") });
                ctr++;
            }
            inLine = new InlineKeyboardMarkup(bts);
        });
    }
}
