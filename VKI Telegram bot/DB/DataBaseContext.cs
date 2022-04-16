using Microsoft.EntityFrameworkCore;
using VKI_Telegram_bot.DB.Entities;

namespace VKI_Telegram_bot.DB
{
    internal class DataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ParserData> ParserDataes { get; set; }
        

        public DataBaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DataBase.db");
        }
    }
}
