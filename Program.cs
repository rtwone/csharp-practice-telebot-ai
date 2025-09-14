using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using GenerativeAI;
using Figgle;

var googleAI = new GoogleAi("AIzaSyDIdn9upFzwv2PuIl4OR7zlRHywujZ_uxI");
var model = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");

Console.Clear();

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("8395024539:AAGdaPnqG5ADloAQqiitZqKhN1DFtI5LHYo", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

// Tampilkan teks ASCII "WaBot MD"
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(@"  ____ _           _   ____        _      _   _____ 
 / ___| |__   __ _| |_| __ )  ___ | |_   / \ |_____|
| |   | '_ \ / _` | __|  _ \ / _ \| __| / _ \  | | 
| |___| | | | (_| | |_| |_) | (_) | |_ / /_\ \_| |_
 \____|_| |_|\__,_|\__|____/ \___/\___|_/   \_\____|
                                                   
");
Console.ResetColor();


// Informasi tambahan
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
Console.WriteLine("+62 898-8808-885");
Console.ResetColor();
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
    return Task.CompletedTask;
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    // Memberitahu di Log bahwa ada yang Mengirimkan Pesan
    DateTime sekarang = DateTime.Now;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write(@"[{0}] ", sekarang.ToString("dd/MM/yyyy HH:mm:ss"));
    Console.ResetColor();
    Console.Write("{0} Mengirimkan Pesan ", msg.From);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("{0}", msg.Text);
    Console.ResetColor();
    // End Log

    if (msg.Text == "/start")
    {
        await bot.SendMessage(msg.Chat, "Haloo, ini adalah AI ChatBot hasil Latihan dari Irfan, silahkan kirimkan pesan anda!");
    }
    else
    {
        var chatSession = model.StartChat();
        await chatSession.GenerateContentAsync("Kamu adalah ChatBot AI yang dibuat Oleh Irfan Hariyanto sebagai Projek Latihannya di Bahasa C#. Gunakan Bahasa Indonesia");
        var response = await chatSession.GenerateContentAsync(msg.Text ?? "Hai");
        await bot.SendMessage(msg.Chat, response.Text() ?? "Maaf, saya tidak bisa menghasilkan jawaban.");
    }
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data}");
        await bot.SendMessage(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}
