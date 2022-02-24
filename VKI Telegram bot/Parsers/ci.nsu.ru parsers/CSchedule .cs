namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class CSchedule : Parser
    {
        public List<List<string>> list = new List<List<string>>();
        public CSchedule(string url = "https://ci.nsu.ru/education/raspisanie-zvonkov/") : base(url)
        {
            _ = Update(new List<List<string>>());
        }
        public bool Update(List<List<string>> list2)
        {
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
            if (list.Count == list2.Count && list[0].Count == list2[0].Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
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
