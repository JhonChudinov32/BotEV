using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ApiAiSDK;
using System.Data.SqlClient;

namespace BotEV
{
    class Program
    {
        static TelegramBotClient Bot;
        static ApiAi apiAi;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1679998384:AAF46ccTDlJbyOonEJQVt0mPo1BYsm30ZHI");
            AIConfiguration config = new AIConfiguration("? //нужно id для подключения заготовленных фраз", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);
            Bot.OnMessage += BotOnMessageRecieved;
            Bot.OnCallbackQuery += BotOnCallbackQueryRecieved;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        private static async void BotOnCallbackQueryRecieved(object sender, CallbackQueryEventArgs e)
        {
            //Обработка событий при нажатии на кнопки из меню

            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {buttonText}");
            if (buttonText == "Зига")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "https://yandex.ru/images/search?pos=8&img_url=https%3A%2F%2Fcs11.pikabu.ru%2Fpost_img%2F2019%2F10%2F18%2F11%2Fog_og_1571422832212815097.jpg&text=%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD%D0%BA%D0%B0%20%D0%B7%D0%B8%D0%B3%D0%B0&lr=20712&rpt=simage&source=wiz");
            }
            else if (buttonText == "Видео")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "https://yandex.ru/video/preview/?text=%D0%BF%D0%BB%D1%8F%D1%88%D1%83%D1%89%D0%B8%D0%B5%20%D0%B5%D0%B2%D1%80%D0%B5%D0%B8&path=wizard&parent-reqid=1614777769333477-301486614053859256200110-production-app-host-man-web-yp-221&wiz_type=vital&filmId=10807997979764438504");
            }
            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");

        }
        private static async void BotOnMessageRecieved(object sender, MessageEventArgs e)
        {
            DateTime datesost = DateTime.Now;
            // DateTime datesost = DateTime.Now;
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} сообщение отправил: {message.Text}");
            // mainConnection.Open();
            // внесение данных в БД
            SqlConnection mainConnection = new SqlConnection("Data Source=ODP-CHUDINOVEM;Initial Catalog=Botmanager;Integrated Security=True");
            mainConnection.Open();
            SqlCommand cmd = new SqlCommand(@"INSERT INTO dbo.Bot(name, messageText, date) VALUES (@attr0,@attr,@attr1)", mainConnection);
            cmd.Parameters.AddWithValue("@attr0", name);
            cmd.Parameters.AddWithValue("@attr", message.Text);
            cmd.Parameters.AddWithValue("@attr1", datesost);

            cmd.ExecuteNonQuery();

            mainConnection.Close();
            switch (message.Text)
            {
                case "/start":
                    string Text =
@"Список команд:
/start - запуск бота;
/inline - вывод меню;
/keyboard - вывод клавиатуры.";
                    await Bot.SendTextMessageAsync(message.From.Id, Text);
                    break;
                case "/inline":
                    var InlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/djonbig"),
                            InlineKeyboardButton.WithUrl("Telegram", "https://t.me/EvgeniyMirniy")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Картинка"),
                            InlineKeyboardButton.WithCallbackData("Видео")
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт в меню", replyMarkup: InlineKeyboard);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Привет"),
                            new KeyboardButton("Как дела?")
                        },
                        new[]
                        {
                            new KeyboardButton("Контакт"){RequestContact = true},
                            new KeyboardButton("Геолокация") {RequestLocation = true}
                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Cooбщение", replyMarkup: replyKeyboard);
                    break;
                default:

                    string answer = "";
                    if (message.Text == "Привет" || message.Text == "Приветики" || message.Text == "Хэй" || message.Text == "Зиг хайль")
                    {
                        Random r = new Random();
                        string[] hi = { "Привет", "Приветики", "Салют", "Хелоу", "Хай", "Здарова"};
                        int i = r.Next(0, hi.Length);
                        answer = hi[i];
                    }
                    else if (message.Text == "Как дела?" || message.Text == "Как ты?" || message.Text == "How are you?" || message.Text == "Как дела" || message.Text == "Как ты" || message.Text == "How are you")
                    {
                        Random r = new Random();
                        string[] howareyou = { "Отлично а у тебя", "Чотко",  "Супер, как ты?", "Зачетно" };
                        int i = r.Next(0, howareyou.Length);
                        answer = howareyou[i];
                    }
                    else if (message.Text == "Что делаешь?" || message.Text == "Чем занимаешься?" || message.Text == "Чем маешься?" || message.Text == "Что творишь?" || message.Text == "Привет! Чем занимаешься?" || message.Text == "How are you")
                    {
                        Random r = new Random();
                        string[] doyoudo = {  "С тобой общаюсь" };
                        int i = r.Next(0, doyoudo.Length);
                        answer = doyoudo[i];
                    }
                    else
                    {
                        Random r = new Random();
                        string[] error = { "Я не понимаю", "Пиши чотко", "Учи немецкий", "Возьми с полки пирожек" };
                        int i = r.Next(0, error.Length);
                        answer = error[i];
                    }
                    // await Bot.SendTextMessageAsync(message.From.Id, answer);
                    await Bot.SendTextMessageAsync(message.From.Id, answer.ToString());
                    var me = Bot.GetMeAsync().Result;
                    SqlConnection mainCon = new SqlConnection("Data Source=ODP-CHUDINOVEM;Initial Catalog=Botmanager;Integrated Security=True");
                    mainCon.Open();
                    SqlCommand cmd1 = new SqlCommand(@"INSERT INTO dbo.Bot(name, messageText, date) VALUES (@attr0,@attr,@attr1)", mainCon);
                    cmd1.Parameters.AddWithValue("@attr0", me.FirstName);
                    cmd1.Parameters.AddWithValue("@attr", answer);
                    cmd1.Parameters.AddWithValue("@attr1", datesost);

                    cmd1.ExecuteNonQuery();
                    mainCon.Close();
                    break;
            }

        }
    }
}
