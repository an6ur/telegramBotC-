using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;


namespace TG_Bot_12_12_2022
{
    //https://telegrambots.github.io/book/1/quickstart.html
    internal class Program
    {
        // Для иммитации нажатия кнопки мыши
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENT_LEFTDOWN = 0x02;
        public const int MOUSEEVENT_LEFTUP = 0x04;
        public const int MOUSEEVENT_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENT_MIDDLEUP = 0x40;
        public const int MOUSEEVENT_RIGHTDOWN = 0x08;
        public const int MOUSEEVENT_RIGHTUP = 0x10;
        //----------------------------------------------------------------------------------------

        //Для показа сообщений на экране!
        //Call Win32 API for opening a Messagebox (This does not require any other libarys)
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);
        //----------------------------------------------------------------------------------------



        private static ITelegramBotClient clietn;

        static void Main(string[] args)
        {
            clietn = new TelegramBotClient("5964721917:AAH2DhJelHK6Nqg6bxlhMf6L8il2LqJnCrk");
            clietn.StartReceiving(Update, Error);

            Console.WriteLine("Hello World!");

            Client_Hello();

            Console.ReadLine();
        }

        //Приветствие бота! 
        private static async void Client_Hello()
        {
            try
            {
                await clietn.SendTextMessageAsync(422810648, "Ready to work!"); // 422810648 мой ID telegram (@getmyid_bot) 
            }
            catch
            {
                Console.WriteLine("Возникла ошибка в приветствие бота при зупуске");
            }

        }

        static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Ошибка телеграмм API:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}", //ошибка АПИ телеграммы
                _ => exception.ToString() //ошибка иного типа
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            //if (message.Text != null)
            if(update.Type == UpdateType.Message && update?.Message?.Text != null)
            {   
                Console.WriteLine($"{message.Chat.FirstName} | {message.Text}");
                
                await HandleMessage(botClient, update.Message);
                return; 
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
        }

        //----------------------------------------------------------------------------------------

        //Функция обработки сообщений
        async static Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.Text.ToLower().Contains("hello"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Вперед");
                //Process.Start("explorer.exe", "https://coderoad.ru/4580263/%D0%9A%D0%B0%D0%BA-%D0%BE%D1%82%D0%BA%D1%80%D1%8B%D1%82%D1%8C-%D0%B2-%D0%B1%D1%80%D0%B0%D1%83%D0%B7%D0%B5%D1%80%D0%B5-%D0%BF%D0%BE-%D1%83%D0%BC%D0%BE%D0%BB%D1%87%D0%B0%D0%BD%D0%B8%D1%8E-%D0%B2-C");
            }

            if (message.Text.ToLower().Contains("/start"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose comands: /inline | /keyboard");
            }

            if (message.Text == "/inline")
            {
                InlineKeyboardMarkup inlineKeyboard = (new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Buy 50c", "buy_50"), //Первое, что видет пользователь, второе - переменная.
                        InlineKeyboardButton.WithCallbackData("Buy 100c", "buy_100"),//
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Sell 50c", "sell_50"),
                        InlineKeyboardButton.WithCallbackData("Sell 100c", "sell_100"),
                    },
                    new []
                    {
                    InlineKeyboardButton.WithUrl(
                        "Link to Repository",
                        "https://github.com/TelegramBots/Telegram.Bot"
                    ),
                    },
                });
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose inline:", replyMarkup: inlineKeyboard);
                return;
            }

            if (message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup keyboard = new[]
                {
                      new[] {    "Top-Left",   "Top" , "Top-Right"    },
                      new[] {        "Left", "Center", "Right"        },
                      new[] { "Bottom-Left", "Bottom", "Bottom-Right" },
                    };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose:", replyMarkup: keyboard);
                return;
            }
            
            if (message.Text == "Bottom-Right")
            {
                mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
            }

            if (message.Text == "Right")
            {
                //MessageBox((IntPtr)0, "Your Message", "My Message Box", 0);
                MessageBox((IntPtr)0, "Здарова, Ёпта!", "My Message Box", 0);

            }

            if (message.Text == "Top-Left")
            {
                Process.Start("explorer", "C:\\");
            }

            if (message.Text == "Center")
            {
                ReplyKeyboardMarkup keyboard = new[]
                {
                      new[] {      "🔜"      },
                    };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose:", replyMarkup: keyboard);
                return;
            }

            if (message.Text == "🔜")
            {
                mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
            }

            if (message.Text == "Top")
            {
                //int width = 1366;//1920;
                //int height = 768;//1080;

                //using (Bitmap bitmap = new Bitmap(width, height))
                //using (Graphics g = Graphics.FromImage(bitmap))
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    g.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
                //    bitmap.Save(stream, ImageFormat.Png);
                //    stream.Seek(0, SeekOrigin.Begin);
                //    InputOnlineFile file = new InputOnlineFile(stream, "screenshot.png");
                //    await botClient.SendPhotoAsync(message.Chat.Id, file);
                //}

                //using var bitmap = new Bitmap(1920, 1080);
                //using (var g = Graphics.FromImage(bitmap))
                //{
                //    g.CopyFromScreen(0, 0, 0, 0,
                //    bitmap.Size, CopyPixelOperation.SourceCopy);
                //}
                //bitmap.Save("filename.jpg", ImageFormat.Jpeg);

            }

            if (message.Text.ToLower().Contains("start"))
            {
                //"C:\vscode\video-waves.mp4"
                var chatId = message.Chat.Id;
                await using var stream = System.IO.File.OpenRead(@"C:\Users\lizan\OneDrive\Рабочий стол\С#_Examples\doc_2022-12-28_02-36-48.mp4"); //@"C:\Users\lizan\OneDrive\Рабочий стол\С#_Examples\video-waves.mp4"  "/path/to/video-waves.mp4"

                await botClient.SendTextMessageAsync(message.Chat.Id, "Минутку, сейчас напишу.");
                await botClient.SendVideoNoteAsync(chatId, stream, duration: 5, length: 360);

                InlineKeyboardMarkup inlineKeyboard = (new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Для парня", "man"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Для девушки", "girl"),
                    },

                });

                await botClient.SendTextMessageAsync(message.Chat.Id, "Таксь, а меню для парня или девушки отправлять?\nЗабыл сразу спросить )",
                    replyMarkup: inlineKeyboard);
                //replyMarkup: new InlineKeyboardMarkup(
                //InlineKeyboardButton.WithUrl(
                //text: "Check sendMessage method",
                //url: "https://core.telegram.org/bots/api#sendmessage"))
                //);
                return;
            }

            //await botClient.SendTextMessageAsync(message.Chat.Id, $"You said:\n{message.Text}"); //Эхо бота   
        }
        //----------------------------------------------------------------------------------------

        //Функция InlineKeyboard
        async static Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("man"))
            {
                await botClient.SendTextMessageAsync(
                                callbackQuery.Message.Chat.Id,
                                $"Понял"
                                );
                return;
            }
            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"You choose with data: {callbackQuery.Data}"
                );
            return;
        }
        //----------------------------------------------------------------------------------------


        //public void Lclic()
        //{
        //    mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
        //    mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        //}

    }
}
