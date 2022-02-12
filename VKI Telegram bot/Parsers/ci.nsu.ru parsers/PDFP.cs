using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<string[]> returned = new List<string[]>();
        public PDFP(string url) : base(url)
        {
            updater();
        }
        public void updater()
        {
            returned.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                returned.Add(new string[] { i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim(), "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "") });
            }
        }
    }
}
