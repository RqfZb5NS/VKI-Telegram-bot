using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace VKI_Telegram_bot
{
    public static class Log
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Info(string messege)
        {
            logger.Info(messege);
        }
        public static void Info(string messege, Exception ex)
        {
            logger.Info(ex, messege);
        }
        public static void Warn(string messege)
        {
            logger.Warn(messege);
        }
        public static void Warn(string messege, Exception ex)
        {
            logger.Warn(ex, messege);
        }
        public static void Eror(string messege, Exception ex)
        {
            logger.Error(ex, messege);
        }
    }
}
