using Microsoft.Data.Sqlite;
using VKI_Telegram_bot.Parsers;
using VKI_Telegram_bot.Parsers.ci.nsu.ru_parsers;

//var timetable = new PDFP("https://ci.nsu.ru/education/schedule/");
//var sgroup = new PDFP("https://ci.nsu.ru/education/spisok-uchebnykh-grupp/");
//var iertification = new PDFP("https://ci.nsu.ru/education/raspisanie-ekzamenov/");
//var cschedule = new CSchedule();
//foreach (var i in timetable.list)
//{
//    Console.WriteLine($"{ i[0] }\n{ i[1] }");
//}
//Console.WriteLine("\n");
//foreach (var i in sgroup.list)
//{
//    Console.WriteLine($"{ i[0] }\n{ i[1] }");
//}
//Console.WriteLine("\n");
//foreach(var i in cschedule.list)
//{
//    foreach (var j in i)
//    {
//        Console.Write($"{j} ");
//    }
//    Console.WriteLine();
//}
//Console.WriteLine("\n");
//foreach (var i in iertification.list)
//{
//    Console.WriteLine($"{ i[0] }\n{ i[1] }");
//}
using (var connection = new SqliteConnection("Data Source=DataBase.db"))
{
    connection.Open();

    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "INSERT INTO Users (id, admin, name) VALUES (1, 1, 'Вова')";
    int number = command.ExecuteNonQuery();

    Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
}
Console.Read();