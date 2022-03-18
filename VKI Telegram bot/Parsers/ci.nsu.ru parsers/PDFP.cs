using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<List<string>> list = new();
        public PDFP(string url) : base(url)
        {

        }
        public bool Update(List<List<string>> list2)
        {
            list.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string link = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new List<string> { link, name });

            }
            if (list.Count == list2.Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i][0] != list2[i][0] || list[i][1] != list2[i][1])
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
