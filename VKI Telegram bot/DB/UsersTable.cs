using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.DB
{
    public class UsersTable
    {
        string dblocation = "Data Source=DB/DataBase.db";
        public void AddUser(int id, int admin = 0, string name = "")
        {
            using (var connection = new SqliteConnection(dblocation))
            {
                SqliteCommand Command = new SqliteCommand($"INSERT INTO Users(id, admin, name) VALUES({id}, {admin}, '{name}')", connection);
                connection.Open(); // открыть соединение
                Command.ExecuteNonQuery(); // выполнить запрос
                connection.Close();
            }
        }
        public void DelUser(int id)
        {
            using (var connection = new SqliteConnection(dblocation))
            {
                SqliteCommand Command = new SqliteCommand($"DELETE FROM Users WHERE id = {id}", connection);
                connection.Open(); // открыть соединение
                Command.ExecuteNonQuery(); // выполнить запрос
                connection.Close();
            }
        }
        public List<int[]> GetUserList()
        {
            List<int[]> list = new List<int[]>();
            using (var connection = new SqliteConnection(dblocation))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM Users", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            list.Add(new int[] { Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1))});
                        }
                    }
                }
            }
            return list;
        }

    }
}
