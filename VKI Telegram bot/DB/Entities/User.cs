using System.ComponentModel.DataAnnotations;

namespace VKI_Telegram_bot.DB
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public bool BlackList { get; set; }
        public string? Name { get; set; }
    }
}
