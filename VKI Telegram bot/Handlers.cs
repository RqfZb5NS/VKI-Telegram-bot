using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;
using VKI_Telegram_bot;
using VKI_Telegram_bot.Parsers.ci_nsu_ru;
//using NLog;

namespace VKI_Telegram_bot
{
    public class Handlers
    {


        static InlineKeyboardMarkup defaultKB = new(
            new[]
            {
                new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Расписание", "timetable"), InlineKeyboardButton.WithCallbackData("Звонки", "cschedule") },
                new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Списки", "sgroup"), InlineKeyboardButton.WithCallbackData("Аттестация", "iertification") },
            }); //{ ResizeKeyboard = true };
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
            using(VKITGBContext db = new VKITGBContext())
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
                    await db.Users.AddAsync(new DB.User
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
                //"log" => SendLogs(botClient, message),
                _ => botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Меню",
                    replyMarkup: defaultKB
                    )
            };

            //static async Task<Message> SendLogs(ITelegramBotClient botClient, Message message)
            //{
            //    using (StreamReader reader = new StreamReader(path))
            //    {
            //        return await botClient.SendDocumentAsync(chatId: message.Chat.Id, await reader.ReadToEndAsync());
            //    }
                
            //}
        }
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            Log.Info($"Id: {callbackQuery.Message!.Chat.Id}, Text: {callbackQuery.Data}");
            using (VKITGBContext db = new VKITGBContext())
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
                    await db.Users.AddAsync(new DB.User
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
                "main" => botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    "Меню",
                    replyMarkup: defaultKB),
                "timetable" => SendPDF(botClient, callbackQuery, Updater.timetable),
                "sgroup" => SendPDF(botClient, callbackQuery, Updater.sgroup),
                "iertification" => SendPDF(botClient, callbackQuery, Updater.iertification),
                "cschedule" => botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id, 
                    callbackQuery.Message.MessageId, 
                    "Звонки", 
                    replyMarkup: Updater.schedule.InLine),
                _ => null,
            };

            static async Task<Message> SendPDF(ITelegramBotClient botClient, CallbackQuery callbackQuery, PDFParser pdfp)
            {
                if (callbackQuery.Data!.Split(' ').Length == 1)
                {
                    return await botClient.EditMessageTextAsync(
                        callbackQuery.Message!.Chat.Id,
                        callbackQuery.Message.MessageId,
                        pdfp.name,
                        replyMarkup: pdfp.inLine
                        );
                }
                else
                {
                    await botClient.EditMessageTextAsync(
                        callbackQuery.Message!.Chat.Id,
                        callbackQuery.Message.MessageId,
                        "Меню",
                        replyMarkup: defaultKB
                        );
                    return await SendDocument(botClient,
                        callbackQuery.Message!,
                        pdfp.list[Convert.ToInt32(callbackQuery.Data!.Split(' ')[1])][1]
                        );
                }
            }
            static async Task<Message> SendDocument(ITelegramBotClient botClient, Message message, string link) // string name
            {

                return await botClient.SendDocumentAsync(
                    chatId: message.Chat.Id,
                    document: new InputOnlineFile(link)
                    );
            }
        }
        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Log.Warn($"Unknown update type: {update.Type}");
            //Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
