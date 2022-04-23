using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramBotExperiments
{
	class Program
	{
		static ITelegramBotClient bot = new TelegramBotClient("5150375839:AAED-xI8lx6XaZjtEFXILpCcDYtVrNZtKsU");
		public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			// Некоторые действия
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
			if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
			{
				var message = update.Message;
				if (message.Text.ToLower() == "/start")
				{
					await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
					return;
				}
				if (message.Text.ToLower() == "stic")
				{
					await botClient.SendStickerAsync(message.Chat, "https://tlgrm.ru/_/stickers/ccd/a8d/ccda8d5d-d492-4393-8bb7-e33f77c24907/1.webp");
				}
				else if (message.Text.ToLower() == "hi" || message.Text.ToLower() == "hello" || message.Text.ToLower() == "привет")
				{
					await botClient.SendTextMessageAsync(message.Chat, "Hello");
				}
				else
				{
					await botClient.SendTextMessageAsync(message.Chat, "");
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			// Некоторые действия
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
				AllowedUpdates = { }, // receive all update types
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