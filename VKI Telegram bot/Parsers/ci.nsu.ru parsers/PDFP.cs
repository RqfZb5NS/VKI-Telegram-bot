using System.Linq;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class PDFP : Parser
    {
        public List<string[]> list = new List<string[]>();
        public string tablename = string.Empty;
        public PDFP(string url, string tn) : base(url)
        {
            tablename = tn;
            foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
            {
                string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                string href = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                list.Add(new string[] { name, href });
            }
        }
        public bool Updater()
        {
            list.Clear();
            var reader = Read(tablename);
            if (reader.HasRows)
            {
                List<string[]> listbd = new List<string[]>();
                while (reader.Read())   // построчно считываем данные
                {
                    listbd.Add(new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString() });
                }
                foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
                {
                    string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                    string href = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    list.Add(new string[] { name, href });
                }
                if (listbd.SequenceEqual(list))
                {
                    list = new List<string[]>(listbd);
                    return false;
                }
                else
                {
                    TableClear();
                    foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
                    {
                        string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                        string href = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                        AddElement(name, href);
                        list.Add(new string[] { name, href });
                    }
                    return true;
                }
            }
            else
            {
                foreach (HtmlAgilityPack.HtmlNode i in doc.DocumentNode.SelectNodes(".//div[@class='file-div']"))
                {
                    string name = i.SelectSingleNode(".//div[@class='file-name']").InnerText.Trim();
                    string href = "https://ci.nsu.ru" + i.SelectSingleNode(".//a").GetAttributeValue("href", "");
                    AddElement(name, href);
                    list.Add(new string[] { name, href });
                }
                return true;
            }
        }
        public void AddElement(string name, string data)
        {
            Command($"INSERT INTO {tablename}(name, data) VALUES('{name}', '{data}')");
        }
        public void TableClear()
        {
            TableClear(tablename);
        }
    }
}
