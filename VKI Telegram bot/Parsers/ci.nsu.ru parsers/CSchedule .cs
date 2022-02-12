using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class CSchedule : Parser
    {
        public List<List<string>> returned = new List<List<string>>();
        public CSchedule(string url = "https://ci.nsu.ru/education/raspisanie-zvonkov/") : base(url)
        {
            updater();
        }
        public void updater()
        {
            returned.Clear();
            int ctr = 0;
            foreach (var i in doc.DocumentNode.SelectSingleNode(".//table[@class='table']").SelectNodes(".//tr"))
            {
                returned.Add(new List<string>());
                foreach (var j in i.SelectNodes(".//p"))
                {
                    returned[ctr].Add(j.InnerText.Trim());
                }
                ctr++;
            }
        }
    }
}
