namespace VKI_Telegram_bot.DB
{
    public class UsersTable : Table
    {
        public List<int[]> list = new List<int[]>();
        public string tablename = "Users";
        public UsersTable()
        {
            UpdateUserList();
        }
        public void AddUser(int id, int admin = 0, string name = "")
        {
            Command($"INSERT INTO {tablename}(id, admin, name) VALUES({id}, {admin}, '{name}')");
        }
        public void DelUser(int id)
        {
            Command($"DELETE FROM {tablename} WHERE id = {id}");
        }
        public List<int[]> UpdateUserList()
        {
            list.Clear();
            var reader = Read(tablename);
            if (reader.HasRows)
            {
                while (reader.Read())   // построчно считываем данные
                {
                    list.Add(new int[] { Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1))});
                }
            }
            return list;
        }
    }
}
