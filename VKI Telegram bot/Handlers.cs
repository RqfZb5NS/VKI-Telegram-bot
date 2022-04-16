using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;
using User = VKI_Telegram_bot.DB.Entities.User;

//using NLog;

namespace VKI_Telegram_bot
{
    public class Handlers
    {
        

        static ReplyKeyboardMarkup defaultKB = new(
            new[]
            {
                new KeyboardButton[] { "Расписание", "Звонки"},
                new KeyboardButton[] { "Списки", "Аттестация" },
            }) { ResizeKeyboard = true };
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                /*UpdateType.MyChatMember => BotOnMyChatMemberReceived(botClient, update.MyChatMember!),*/
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
                return;
            Log.Info($"Id: {message.Chat.Id}, Text: {message.Text}");
            using(DataBaseContext db = new DataBaseContext())
            {
                if (db.Users.Find(message.Chat.Id) != null)
                {
                    if (db.Users.Find(message.Chat.Id)!.BlackList)
                    {
                        return;
                    }
                }
                else
                {
                    await db.Users.AddAsync(new User
                    {
                        Id = message.Chat.Id,
                        Name = $"{message.Chat.FirstName} {message.Chat.LastName}",
                        BlackList = false,
                    });
                    await db.SaveChangesAsync();
                }
            }
            _ = message.Text!.Split(' ')[0] switch
            {
                "Расписание" => SendInlineKeyboard(botClient, message, Updater.timetable.inLine!, "Выберите:"),
                "Звонки" => SendInlineKeyboard(botClient, message, Updater.schedule.inLine!, "Расписание звонков:"),
                "Списки" => SendInlineKeyboard(botClient, message, Updater.sgroup.inLine!, "Выберите:"),
                "Аттестация" => SendInlineKeyboard(botClient, message, Updater.iertification.inLine!, "Выберите:"),
                _ => SendKeyboard(botClient, message, defaultKB)
            };
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, InlineKeyboardMarkup kb, string text)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: text,
                                                            replyMarkup: kb
                                                            );
            }

            static async Task<Message> SendKeyboard(ITelegramBotClient botClient, Message message, ReplyKeyboardMarkup kb)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Выберите:",
                                                            replyMarkup: kb);
            }
        }
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            Log.Info($"Id: {callbackQuery.Message!.Chat.Id}, Text: {callbackQuery.Data}");
            using (DataBaseContext db = new DataBaseContext())
            {
                if (db.Users.Find(callbackQuery.Message!.Chat.Id) != null)
                {
                    if (db.Users.Find(callbackQuery.Message.Chat.Id)!.BlackList)
                    {
                        return;
                    }
                }
                else
                {
                    await db.Users.AddAsync(new User
                    {
                        Id = callbackQuery.Message.Chat.Id,
                        Name = $"{callbackQuery.Message.Chat.FirstName} {callbackQuery.Message.Chat.LastName}",
                        BlackList = false,
                    });
                    await db.SaveChangesAsync();
                }
            }
            //Console.WriteLine($"Id: {callbackQuery.Message.Chat.Id}, CallbackQuery: {callbackQuery.Data}");
            _ = callbackQuery.Data!.Split(' ')[0] switch
            {
                "timetable" => SendPdf(botClient, callbackQuery, Updater.timetable.list),
                "sgroup" => SendPdf(botClient, callbackQuery, Updater.sgroup.list),
                "iertification" => SendPdf(botClient, callbackQuery, Updater.iertification.list),
                _ => null,
            };

            static async Task<Message> SendPdf(ITelegramBotClient botClient, CallbackQuery callbackQuery, List<List<string>> list)
            {
                return await SendDocument(botClient, 
                    callbackQuery.Message!,
                    list[Convert.ToInt32(callbackQuery.Data!.Split(' ')[1])][1]
                    );
            }
            static async Task<Message> SendDocument(ITelegramBotClient botClient, Message message, string link) // string name
            {

                return await botClient.SendDocumentAsync(
                    chatId: message.Chat.Id,
                    document: new InputOnlineFile(link)
                    );
            }
        }
        /*private static Task BotOnMyChatMemberReceived(ITelegramBotClient botClient, ChatMemberUpdated chatMemberUpdated)
        {
            //chatMemberUpdated.NewChatMember
            return Task.CompletedTask;
        }*/
        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Log.Warn($"Unknown update type: {update.Type}");
            //Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
