using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace VKI_Telegram_bot.DB
{
    public class ParserData
    {
        [Key]
        public string? Name { get; set; }
        public string? JSonData { get; set; }
        public List<List<string>> GetDataAsList()
        {
            if (JSonData != null) { return  JsonSerializer.Deserialize<List<List<string>>>(JSonData); }
            else { return new List<List<string>>(); }
        }
        public void SetDataFromList(List<List<string>> list) => JSonData = JsonSerializer.Serialize(list);
        public bool Compare(ParserData parserData)
        {
            return parserData.JSonData == JSonData;
        }
    }
}