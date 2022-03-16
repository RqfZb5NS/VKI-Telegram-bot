using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<PDFPEntity> list = new();
        public PDFP(string url) : base(url)
        {

        }
        public bool Update(List<PDFPEntity> list2)
        {
            list.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string _name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string _link = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new PDFPEntity {Name = _name, Link = _link});

            }
            if (list.Count == list2.Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Link != list2[i].Link || list[i].Name != list2[i].Name)
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
