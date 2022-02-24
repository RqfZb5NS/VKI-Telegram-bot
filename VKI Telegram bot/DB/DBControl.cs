using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;

namespace VKI_Telegram_bot.DB
{
    public class DBControl
    {
        public readonly string databasefilelocation = "DB/DataBase.db";

        public SqliteCommand SQLComnd(string _command)
        {
            using (var connection = new SqliteConnection("Data Source=" + databasefilelocation))
            {
                SqliteCommand command = new SqliteCommand(_command, connection);
                connection.Open(); // открыть соединение
                command.ExecuteNonQuery(); // выполнить запрос
                connection.Close();
                return command;
            }
        }
        public SqliteDataReader GetReader(string nametable)
        {
            var command = SQLComnd("SELECT * FROM " + nametable);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                return reader;
            }
        }
        public List<List<string>> GetList(string nametable)
        {
            GetReader(nametable);
        }
        public void ClearTable(string tablename)
        {
            SQLComnd($"DELETE FROM {tablename}");
        }
    }
}
