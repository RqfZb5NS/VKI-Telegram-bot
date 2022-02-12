using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers
{
    public class CSchedule : Parser
    {
        public List<string[]> returned = new List<string[]>();
        public CSchedule(string url = "https://ci.nsu.ru/education/raspisanie-zvonkov/") : base(url)
        {
            updater();
        }
        public void updater()
        {
            returned.Clear();
            //doc.DocumentNode.SelectSingleNode(".//table[@class='table']");
            //class="table"
        }
    }
}
