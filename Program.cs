using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramBotExperiments
{
	class Program
	{
		static ITelegramBotClient bot = new TelegramBotClient("5365199670:AAEt_XClVQH_oCJpsQ1PO6nZqdErZuqWDLw");
		private static string joke;
		private static GetRequest request = new GetRequest("https://geek-jokes.sameerkumar.website/api?format=json");
		public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
			if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
			{
				var message = update.Message;
				if (message.Text.ToLower() == "/start")
				{
					await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт!");
				}
				else if (message.Text.ToLower() == "hi" || message.Text.ToLower() == "привет")
				{
					await botClient.SendTextMessageAsync(message.Chat, "Hello");
				}
				else if (message.Text.ToLower() == "/help")
				{
					await botClient.SendTextMessageAsync(message.Chat, $"1./start \n2.hi / привет \n3./joke ");
				}
				else if (message.Text.ToLower() == "/joke")
				{
					request.Run();
					var response = request.Response;
					var json = JObject.Parse(response);
					joke = json["joke"].ToString();
					await botClient.SendTextMessageAsync(message.Chat, joke);
				}
				else
				{
					await botClient.SendTextMessageAsync(message.Chat, "if you need help use: /help");
				}
			}
		}
		//---------------------------------------------------------------------------------------------------------------------------------------------- 

		public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
			return Task.CompletedTask;
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
			var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { },
			};
			bot.StartReceiving(
			 HandleUpdateAsync,
			 HandleErrorAsync,
			 receiverOptions,
			 cancellationToken
			);
			Console.ReadLine();
		}
	}
}