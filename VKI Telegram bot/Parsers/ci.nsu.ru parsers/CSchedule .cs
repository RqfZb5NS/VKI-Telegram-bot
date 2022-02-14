namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class CSchedule : Parser
    {
        public List<List<string>> list = new List<List<string>>();
        public CSchedule(string url = "https://ci.nsu.ru/education/raspisanie-zvonkov/") : base(url)
        {
            Updater();
        }
        public void Updater()
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
        }
    }
}
