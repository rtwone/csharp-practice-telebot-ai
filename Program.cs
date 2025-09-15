using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using GenerativeAI;
using Figgle;
using System.Collections.Generic;

namespace LatihanBotTele
{
    class UserData
    {
        public long UserId { get; set; }
        public bool FirstMessage { get; set; }
    }
    class Program
    {
        private static GoogleAi googleAI = null!;
        private static GenerativeModel model = null!;
        private static TelegramBotClient bot = null!;


        // Simpan user berdasarkan id
        private static Dictionary<long, UserData> users = new Dictionary<long, UserData>();

        static async Task Main(string[] args)
        {
            Console.Clear();
            googleAI = new GoogleAi("AIzaSyCSr-ogZsxryTKVM0WbLIEOvmC9kHqqjRM");
            model = googleAI.CreateGenerativeModel("models/gemini-2.5-flash");

            using var cts = new CancellationTokenSource();
            bot = new TelegramBotClient("8395024539:AAGdaPnqG5ADloAQqiitZqKhN1DFtI5LHYo", cancellationToken: cts.Token);

            var me = await bot.GetMe();
            bot.OnError += OnError;
            bot.OnMessage += OnMessage;
            // bot.OnUpdate += OnUpdate;

            // Banner ASCII
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"  ____ _           _   ____        _      _   _____ 
 / ___| |__   __ _| |_| __ )  ___ | |_   / \ |_____|
| |   | '_ \ / _` | __|  _ \ / _ \| __| / _ \  | | 
| |___| | | | (_| | |_| |_) | (_) | |_ / /_\ \_| |_
 \____|_| |_|\__,_|\__|____/ \___/\___|_/   \_\____|
                                                   
");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n       [ Created By Irfan ]");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ChatbotAI - Practice C#");
            Console.ResetColor();
            Console.Write(" : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Telegram Bot");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Follow Insta Dev");
            Console.ResetColor();
            Console.Write(" : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("@irfvnny");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Message Me On WhatsApp");
            Console.ResetColor();
            Console.Write(" : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("+62 898-8808-885\n");
            Console.ResetColor();

            Console.ReadLine();
            cts.Cancel(); // stop the bot
        }

        // handle errors
        private static Task OnError(Exception exception, HandleErrorSource source)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }

        // handle message
        private static async Task OnMessage(Message msg, UpdateType type)
        {
            long userId = msg.From?.Id ?? 0;

            if (!users.ContainsKey(userId))
            {
                users[userId] = new UserData { UserId = userId, FirstMessage = false };
                Console.WriteLine($"User baru ditambahkan: {userId}");
            }
            var userData = users[userId];

            DateTime sekarang = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(@"[{0}] ", sekarang.ToString("dd/MM/yyyy HH:mm:ss"));
            Console.ResetColor();
            Console.Write("{0} Mengirimkan Pesan ", arg0: msg.From?.Username);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", msg.Text);
            Console.ResetColor();

            if (msg.Text == "/start")
            {
                await bot!.SendMessage(msg.Chat, "Haloo, ini adalah AI ChatBot hasil Latihan dari Irfan, silahkan kirimkan pesan anda!");
            }
            else
            {
                var chatSession = model.StartChat();
                if (!userData.FirstMessage)
                {
                    await chatSession.GenerateContentAsync("Kamu adalah ChatBot AI yang dibuat Oleh Irfan Hariyanto sebagai Projek Latihannya di Bahasa C#. " +
                                                           "Gunakan Bahasa Indonesia\n" +
                                                           "Dan jika kamu merespon pengguna, kamu jawab dengan bahasa manusia saja, dan seperti sudah akrab dengan pengguna. " +
                                                           "Kurangi format-format teks seperti bold/italic/underline, kirim teks biasa saja. " +
                                                           "Ini username Telegram saya: @" + msg.From?.Username + " dan ini Id saya: " + msg.From?.Id);

                    userData.FirstMessage = true;
                    Console.WriteLine("Pengguna " + msg.From?.Id + " sudah memulai percakapan pertama!");
                }
                var response = await chatSession.GenerateContentAsync(msg.Text ?? "Hai");
                await bot!.SendMessage(msg.Chat, response.Text() ?? "Maaf, saya tidak bisa menghasilkan jawaban.");
            }
        }
    }
}
