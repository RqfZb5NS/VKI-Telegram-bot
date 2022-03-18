using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.DB
{
    public class Data
    {
        [Key]
        public string? Name { get; set; }
        public string? JSonData { get; set; }
    }
}
