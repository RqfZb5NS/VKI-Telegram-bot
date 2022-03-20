using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Telegram
{
    public class Handlers
    {
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
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:                
                //UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                //_ => UnknownUpdateHandlerAsync(botClient, update)
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),

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
            //Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/start" => AddUser(botClient, message),
                "Расписание" => SendInlineKeyboard(botClient, message, Updater.timetable.InLine),
                "Списки" => SendInlineKeyboard(botClient, message, Updater.sgroup.InLine),
                _ => SendKeyboard(botClient, message, new(
                    new[]
                    {
                        new KeyboardButton[] { "Расписание", "Звонки"},
                        new KeyboardButton[] { "Списки" },
                    })
                { ResizeKeyboard = true })
            };
            static async Task<Message> AddUser(ITelegramBotClient botClient, Message message)
            {
                using (VKITGBContext db = new VKITGBContext())
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
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                        new KeyboardButton[] { "Расписание", "Звонки"},
                        new KeyboardButton[] { "Списки" },
                    })
                {
                    ResizeKeyboard = true
                };
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Выберите:",
                                                            replyMarkup: replyKeyboardMarkup);
            }
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, InlineKeyboardMarkup kb)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Выберите:",
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
            var action = callbackQuery.Data.Split(' ')[0] switch
            {
                "timetable" => SendTimetable(botClient, callbackQuery),
            };

            static async Task<Message> SendTimetable(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                return await SendDocument(botClient, 
                    callbackQuery.Message, 
                    //Updater.timetable.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][0],
                    Updater.timetable.list[Convert.ToInt32(callbackQuery.Data.Split(' ')[1])][1]);
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
