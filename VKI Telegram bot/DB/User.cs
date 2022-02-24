namespace VKI_Telegram_bot.DB
{
    public class User
    {
        public int Id { get; set; }
        public bool Admin { get; set; }
        public string Name { get; set; }
        public User(int _Id, bool _Admin = false, string _Name = "NoName")
        {
            Id = _Id;
            Admin = _Admin;
            Name = _Name;
        }
    }
}
