using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VKI_Telegram_bot
{
    public struct Settings
    {
        public string BotApiKey { get; set; }
        public int UpdaterAwait { get; set; }
    }
    public static class AppSettings
    {
        public static Settings settings { get; set; }
        static AppSettings()
        {
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appsettings.json"));
        }
    }
}