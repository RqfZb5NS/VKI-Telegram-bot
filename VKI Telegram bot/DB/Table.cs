using Microsoft.Data.Sqlite;

namespace VKI_Telegram_bot.DB
{
    public class Table
    {
        string dblocation = "Data Source=DB/DataBase.db";
        public SqliteCommand Command(string comnd)
        {
            using (var connection = new SqliteConnection(dblocation))
            {
                SqliteCommand command = new SqliteCommand(comnd, connection);
                connection.Open(); // открыть соединение
                command.ExecuteNonQuery(); // выполнить запрос
                connection.Close();
                return command;
            }
        }
        public SqliteDataReader Read(string nametable)
        {
            var command = Command("SELECT * FROM " + nametable);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                return reader;
            }
        }
        public void TableClear(string tablename)
        {
            Command($"DELETE FROM {tablename}");
        }
    }
}
