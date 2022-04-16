using System.Text.Json;

namespace VKI_Telegram_bot
{
    public struct Settings
    {
        public string BotApiKey { get; set; }
        public int UpdaterAwait { get; set; }
        public long[] Admins { get; set; }
    }
    public static class AppSettings
    {
        public static Settings Settings { get; set; }
        static AppSettings()
        {
            Settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appsettings.json"));
        }
    }
}