namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<List<string>> list = new();
        string name = null;
        public PDFP(string url, string _name) : base(url)
        {
            name = _name;
            _ = Update(new List<List<string>>());
        }
        public bool Update(List<List<string>> list2)
        {
            list.Clear();
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string href = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new List<string> { name, href });

            }
            //return list.SequenceEqual(list2);
            if (list.Count == list2.Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (list[i][j] != list2[i][j])
                        {
                            return true;
                        }
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
