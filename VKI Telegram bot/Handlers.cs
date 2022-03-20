using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;
using VKI_Telegram_bot;

namespace VKI_Telegram_bot.Telegram
{
    public class Handlers
    {
        static ReplyKeyboardMarkup defaultKB = new(
                    new[]
                    {
                        new KeyboardButton[] { "Расписание", "Звонки"},
                        new KeyboardButton[] { "Списки" },
                    })
        {
            ResizeKeyboard = true
        };
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                //UpdateType.Poll:                
                //UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                //_ => UnknownUpdateHandlerAsync(botClient, update)
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
            Console.WriteLine($"Id: {message.Chat.Id}, Text: {message.Text}");
            using(VKITGBContext db = new VKITGBContext())
            {
                if (db.Users.Find(message.Chat.Id) == null)
                {
                    db.Users.Add(new DB.User
                    {
                        Id = message.Chat.Id,
                        Name = $"{message.Chat.FirstName} {message.Chat.LastName}",
                        Admin = false
                    }
                    );
                }
                db.SaveChanges();
            }
            var action = message.Text!.Split(' ')[0] switch
            {
                "Расписание" => SendInlineKeyboard(botClient, message, Program.timetable.InLine, "Выберите:"),
                "Звонки" => SendInlineKeyboard(botClient, message, Program.cschedule.InLine, "Расписание звонков:"),
                "Списки" => SendInlineKeyboard(botClient, message, Program.sgroup.InLine, "Выберите:"),
                _ => SendKeyboard(botClient, message, defaultKB)
            };
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, InlineKeyboardMarkup kb, string text)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
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
            Console.WriteLine($"Id: {callbackQuery.Message.Chat.Id}, CallbackQuery: {callbackQuery.Data}");
            var action = callbackQuery.Data.Split(' ')[0] switch
            {
                "timetable" => SendTimetable(botClient, callbackQuery),
                "sgroup" => SendSgroup(botClient, callbackQuery),
                _ => null,
            };

            static async Task<Message> SendTimetable(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                return await SendDocument(botClient, 
                    callbackQuery.Message,
                    //Updater.timetable.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][0],
                    Program.timetable.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][1]);
            }
            static async Task<Message> SendSgroup(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                return await SendDocument(botClient,
                    callbackQuery.Message,
                    //Updater.timetable.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][0],
                    Program.sgroup.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][1]);
            }
            static async Task<Message> SendDocument(ITelegramBotClient botClient, Message message, string link) // string name
            {

                return await botClient.SendDocumentAsync(
                    chatId: message.Chat.Id,
                    document: new InputOnlineFile(link));
            }
        }
    }
}
