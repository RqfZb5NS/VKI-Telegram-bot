using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers
{
    public abstract class Parser : GetHtml
    {
        public HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        public Parser(string url) : base(url)
        {
            doc.LoadHtml(html);
        }
    }
}
