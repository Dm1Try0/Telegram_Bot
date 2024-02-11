using Microsoft.Extensions.DependencyInjection;
using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Dtos;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ref.Bot.Business.Internals
{
	internal class TelegramService : ITelegramService
	{
		private static IServiceProvider _serviceProvider;
		private static bool IsStarting = false;
		private readonly ITelegramBotClient _botClient;
		public TelegramService(ITelegramBotClient botClient)
		{
			_botClient = botClient;

		}
		// Примечание: Данный код разрабатывался слишком давно(2022.11), решил не производить код ревью, много решений которые было допущены по неопытности
		//Например весь этот файл)
		// 
		public void StartReceiving(IServiceProvider serviceProvider)
		{
			if (IsStarting)
			{
				return;
			}
			_serviceProvider = serviceProvider;
			IsStarting = true;
			var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { },
			};
			_botClient.StartReceiving(
				HandleUpdateAsync,
				HandleErrorAsync,
				receiverOptions,
				cancellationToken: cts.Token);
		}

		private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			try
			{
				if (update.Type == UpdateType.CallbackQuery)
				{
					HandleCallbackQuery(botClient, update.CallbackQuery).Wait();

					return;
				}

				if (update.Type == UpdateType.Message && update?.Message?.Text != null ^ update?.Message?.Photo != null ^ update?.Message?.Video != null)
				{

					HandleMessage(botClient, update.Message).Wait();
					return;

				}
			}
			catch (Exception ex)
			{
				try
				{
					Console.WriteLine(ex);
					return;
				}
				catch
				{
					Console.WriteLine("Ошибочка в обработчике главном!");
					return;
				}
			}
		}
		private async Task HandleMessage(ITelegramBotClient botClient, Message message)
		{
			string admChat = "";//приватный чат для администрации
			if (message.Chat.Id < -100)//заглушка от работы бота в каналах и чатах
			{
				return;
			}
			var user = await GetOrCreate(message.Chat.Id, message.Chat.Username, message.Chat.FirstName);
			if (user.Status == 201)
			{
				return;
			}
			if (message.Chat.Username == null)
			{
				try
				{
					await botClient.SendTextMessageAsync(message.Chat.Id, "Вам нужно поставить себе @username для работы с ботом.\r\n<i>После того как вы установите " +
						"</i><i>@username</i> <i>используйте команду </i><i>/start</i> <i>для отправки заявки.</i>", ParseMode.Html);
					return;
				}
				catch { return; }
			}
			ReplyKeyboardMarkup StartButton = new(new[] {
				new KeyboardButton[]
			{
				$"{char.ConvertFromUtf32(0x26CF)} Ссылки",  $"{char.ConvertFromUtf32(0x1F464)} Личный кабинет",
			 },
				new KeyboardButton[] {$"{char.ConvertFromUtf32(0x1F4C4)} Инструкции",$"{char.ConvertFromUtf32(0x1F51D)} Топ трафферов", $"{char.ConvertFromUtf32(0x1F4BB)} Обратная связь" }, })
			{ ResizeKeyboard = true };
			if (user.Status == 2)
			{
				if (message.Text != "/mynew")
				{
					return;
				}

			}
			if (user.Status == -1)
			{
				return;
			}
			if (message.Text.StartsWith("/start"))
			{
				try
				{

					await botClient.SendTextMessageAsync(message.Chat.Id, $"Главное меню", replyMarkup: StartButton);
					return;
				}
				catch
				{


					return;
				}
			}



			if (user.Username != message.Chat.Username)
			{
				await UpdateUsername(message.Chat.Username, user.TelegramId);
			}
			if (user.First != message.Chat.FirstName)
			{
				await UpdateFirstname(message.Chat.FirstName, user.TelegramId);
			}
			if (user.Status == 200)
			{
				await UStatus(201, user.Id);
				await botClient.SendTextMessageAsync(message.Chat.Id, "Вы заблокированы.");
				return;
			}




			if (message.Text != null)
			{

				try
				{
					if (DateTime.UtcNow > user.LastMessage.AddMinutes(10))
					{
						await SpamNullable(message.Chat.Id);
					}
					if (user.LastMessage.AddSeconds(1) > DateTime.UtcNow)
					{
						await SpamControl(message.Chat.Id);
					}
					await UpdateMessageLast(message.Chat.Id, DateTime.UtcNow);
					if (user.SpamControl == 30)
					{

						await botClient.SendTextMessageAsync($"{admChat}", $"Мелкий спам\nОчки Антифрода: 30\n@{user.Username}\n{user.Id}\n{user.Id}");
						await botClient.SendTextMessageAsync(message.Chat.Id, $"Вы попали в спам контроль.");
						return;
					}
					if (user.SpamControl == 50)
					{
						InlineKeyboardMarkup vote = new(new[] {
					new[]{
					InlineKeyboardButton.WithCallbackData($"Предупреждение",$"warn^{message.Chat.Id}"),
					InlineKeyboardButton.WithCallbackData($"БАН",$"spamban^{user.Id}"),},});
						await botClient.SendTextMessageAsync($"{admChat}", $"Средний спам\nОчки Антифрода: 50\n@{user.Username}\n{user.Id}\n{user.Id}", replyMarkup: vote);
						await botClient.SendTextMessageAsync(message.Chat.Id, $"Дальнейший спам повлечет автоблокировку.");
						return;
					}
					if (user.SpamControl == 100)
					{
						InlineKeyboardMarkup vote = new(new[] {
					new[]{
					InlineKeyboardButton.WithCallbackData($"Предупреждение",$"warn^{message.Chat.Id}"),
					InlineKeyboardButton.WithCallbackData($"БАН",$"spamban^{user.Id}"),},});
						await botClient.SendTextMessageAsync($"{admChat}", $"Пользователь Заблочен\nОчки Антифрода: 100\nПользователь был заблокирован\n\n@{user.Username}\n{user.Id}\n{user.Id}", replyMarkup: vote);
						await UStatus(200, user.Id);
						return;
					}
				}
				catch
				{
					return;
				}

			}

			if (message.Text == "/mynew")
			{
				if (user.Status == 2)
				{
					await UStatus(0, user.Id);
					await botClient.SendTextMessageAsync(message.Chat.Id, "<b>Заполните анкету снова. </b>\n\n<i>Количество заявок ограничено, максимум вы можете отправить 3.</i>", ParseMode.Html);
					return;
				}
				await botClient.SendTextMessageAsync(message.Chat.Id, "Недоступно для вас.");
				return;
			}
			if (user.Status == 0)
			{
				try
				{
					if (user.AnketAmount == 3)
					{
						await botClient.SendTextMessageAsync(message.Chat.Id, "Вы достигли максимального количества заявок,ожидайте пока их рассмотрят.");
						await UStatus(-1, user.Id);
						return;
					}
					await botClient.SendTextMessageAsync(message.Chat.Id, $"{char.ConvertFromUtf32(0x1F525)}<b>Приветствуем вас в боте! Чтобы получить доступ " +
						   $"к боту, инструкциям и общему чату заполните анкету:</b>\n\n<b>1.</b>Расскажите о себе\n " +
						   $"<i>Отправьте анкету одним сообщением, что бы Администратор мог принять вашу заявку. В случае возникновения вопросов Администратор свяжется с вами лично.</i>", parseMode: ParseMode.Html);
					await UStatus(1, user.Id);
					return;
				}
				catch { return; }
			}
			if (user.Status == 1)
			{
				try
				{
					InlineKeyboardMarkup useraccept = new(new[] {
					new[]{
					InlineKeyboardButton.WithCallbackData($"Разрешить доступ",$"acceptUser^{message.Chat.Id}"),
					InlineKeyboardButton.WithCallbackData($"Отклонить",$"declineUser^{message.Chat.Id}"),},});
					await botClient.SendTextMessageAsync(admChat, $"Заявка!\n\n@{user.Username}\nTgId:{user.TelegramId}\nId:{user.Id}\nName:{user.First}\n\n" +
						$"О себе: {message.Text} ", replyMarkup: useraccept);
					await botClient.SendTextMessageAsync(message.Chat.Id, "<b>Ваша заявка отправлена!</b>\n\nОжидайте ответа от Администации\n\nВы можете заполнить заявку заново /mynew\n\nПо любым вопросам: ", parseMode: ParseMode.Html);
					await AnketAmount(user.TelegramId);
					await UStatus(2, user.Id);
					return;
				}
				catch { return; }
			}
			if (user.Status == 2)
			{

				return;
			}


			//ADMINKA
			if (message.Text == "admin" ^ message.Text == "/admin")
			{
				try
				{
					if (user.Role == "Admin")
					{
						InlineKeyboardMarkup admMenu = new(new[] {
					new[]{
					InlineKeyboardButton.WithCallbackData($"{char.ConvertFromUtf32(0x1F4E2)}Проверка юзеров","checkusers"),},
					new[]{
					InlineKeyboardButton.WithCallbackData($"{char.ConvertFromUtf32(0x2705)}Разблокировать юзера{char.ConvertFromUtf32(0x2705)}","acceptUser"),
					InlineKeyboardButton.WithCallbackData($"{char.ConvertFromUtf32(0x274C)}Бан юзера{char.ConvertFromUtf32(0x274C)}","banUser") , },});

						await botClient.SendTextMessageAsync(message.Chat.Id, "Админ меню", replyMarkup: admMenu);
						return;
					}
				}

				catch { return; }
			}
		}

		private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
		{
			var user = await GetOrCreate(callbackQuery.Message.Chat.Id, callbackQuery.Message.Chat.Username, callbackQuery.Message.Chat.FirstName);

			string admChat = "";
			ReplyKeyboardMarkup StartButton = new(new[] {
				new KeyboardButton[]
			{
				$"{char.ConvertFromUtf32(0x1F464)} Кнопка сверху"
			 },
				new KeyboardButton[] {$"{char.ConvertFromUtf32(0x1F4BB)} Кнопка cнизу" }, })
			{ ResizeKeyboard = true };

			if (callbackQuery.Data.StartsWith("acceptUser"))
			{
				try
				{

					var a = await botClient.CreateChatInviteLinkAsync("ссылка", memberLimit: 1);
					InlineKeyboardMarkup link = new(new[] {
					new[]{
					InlineKeyboardButton.WithUrl($"Общий чат",$"{a.InviteLink}"),},});
					string[] chat = callbackQuery.Data.Split('^');
					await botClient.EditMessageReplyMarkupAsync($"{admChat}", callbackQuery.Message.MessageId);
					var getuser = await GetByTelegramId(Convert.ToInt64(chat[1]));
					await UStatus(3, getuser.Id);
					await botClient.SendTextMessageAsync(chat[1], $"<b>Вы приняты и можете приступать к работе, вcтупайте в общий чат</b>\n\n<i>Все инструкции" +
						$" и информация находятся в чате в закреплённом сообщении</i>{char.ConvertFromUtf32(0x1F447)}", ParseMode.Html, replyMarkup: link);
					await botClient.SendTextMessageAsync(chat[1], $"{char.ConvertFromUtf32(0x26CF)} <b>Ссылки</b> - <i>меню для создания реферальных ссылок в наши проекты.</i>" +
						$"\n{char.ConvertFromUtf32(0x1F464)} <b>Личный кабинет</b> - <i>ваша статистика и вывод заработанных средств.</i>" +
						$"\n{char.ConvertFromUtf32(0x1F4C4)} <b>Инструкции</b> - <i>некоторые мануалы с подробным объяснением работы с нами.</i>" +
						$"\n{char.ConvertFromUtf32(0x1F4BB)} <b>Обратная связь</b> - <i>связь с Администрацией.</i> \n{char.ConvertFromUtf32(0x1F51D)} <b>Топ трафферов</b>" +
						$" - <i>топ 10 лучших трафферов по заработку.</i>", parseMode: ParseMode.Html);
					await botClient.SendTextMessageAsync(chat[1], "Главное меню", replyMarkup: StartButton);
					long idfforank = Convert.ToInt64(chat[1]);
					await AnketAmountNull(idfforank);
					return;
				}
				catch { return; }
			}
			if (callbackQuery.Data.StartsWith("declineUser"))
			{
				try
				{
					string[] chat = callbackQuery.Data.Split('^');
					await botClient.EditMessageReplyMarkupAsync($"{admChat}", callbackQuery.Message.MessageId);
					await botClient.SendTextMessageAsync(chat[1], "Ваша заявка была отклонена.");
					return;
				}
				catch { return; }
			}

			if (callbackQuery.Data.StartsWith("spamban^"))
			{
				try
				{
					string[] words = callbackQuery.Data.Split("^");
					long userid = Convert.ToInt64(words[1]);
					long chat = Convert.ToInt64(admChat);
					await UStatus(200, userid);
					await botClient.SendTextMessageAsync(chat, "Вы заблокировали пользователя");
					return;
				}
				catch
				{
					return;
				}

			}
			if (callbackQuery.Data.StartsWith("warn^"))
			{
				try
				{
					string[] words = callbackQuery.Data.Split("^");
					long userid = Convert.ToInt64(words[1]);
					await botClient.SendTextMessageAsync(words[1], "Администратор отправил вам предупреждение");

					return;
				}
				catch
				{
					return;
				}

			}

			return;
		}
		private List<long> GetAllTgId()
		{
			var usersService = GetUsersService();
			var result = usersService.GetAllTelegramId();
			return result;
		}
		private async Task<UserDto> GetByUsername(string username)
		{
			var usersService = GetUsersService();
			var result = usersService.GetByUsername(username);

			return result;
		}
		private async Task<UserDto> GetByTelegramId(long telegramid)
		{
			var usersService = GetUsersService();
			var result = usersService.Get(telegramid);

			return result;
		}
		private async Task<UserDto> GetOrCreate(long telegramId, string username, string firstName)
		{
			var usersService = GetUsersService();
			var user = usersService.Get(telegramId);
			if (user is null)
			{
				user = usersService.Create(telegramId, username, firstName);
			}

			return user;
		}
		private async Task UpdateUsername(string username, long telegramid)
		{
			var userService = GetUsersService();
			userService.UpdUsername(username, telegramid);
		}
		private async Task UpdateFirstname(string name, long telegramid)
		{
			var userService = GetUsersService();
			userService.UpdName(name, telegramid);
		}
		private async Task UpdateMessageLast(long telegramid, DateTime time)
		{
			var userService = GetUsersService();
			userService.UpdLastMessage(telegramid, time);
		}

		private async Task IsReferalUpd(long userid)
		{
			var userService = GetUsersService();
			userService.IsReferalUpd(userid);
		}

		private async Task<UserDto> GetByID(long userId)
		{
			var usersService = GetUsersService();
			var result = usersService.GetById(userId);
			return result;
		}
		private async Task UStatus(long status, long userid)
		{
			var usersService = GetUsersService();
			usersService.StatusUpd(status, userid);
		}
		private async Task SpamControl(long telegramid)
		{
			var userService = GetUsersService();
			userService.UpdSpam(telegramid);
		}
		private async Task SpamNullable(long telegramid)
		{
			var userService = GetUsersService();
			userService.SpamNullable(telegramid);
		}
		private async Task<UserDto> GetByTgId(long telegramId)
		{
			var usersService = GetUsersService();
			var user = usersService.Get(telegramId);
			return user;
		}
		private async Task AnketAmount(long tgid)
		{
			var userService = GetUsersService();
			userService.AnketAmountUpdate(tgid);
		}
		private async Task AnketAmountNull(long tgid)
		{
			var userService = GetUsersService();
			userService.AnketAmountNullable(tgid);
		}
		private async Task SortingService(long tgid)
		{
			var sortService = GetSortService();
			sortService.SortUsersForTop(tgid);
		}
		private async Task<CommonDto> GetCommonRefUser(string id)
		{
			var commonService = GetCommonService();
			var user = commonService.GetByIdInfo(id);
			return user;
		}
		private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
		}
		private IUsersService GetUsersService()
		{
			return _serviceProvider.GetService<IUsersService>();
		}
		private ICommonService GetCommonService()
		{
			return _serviceProvider.GetService<ICommonService>();
		}
		private ISortedTop GetSortService()
		{
			return _serviceProvider.GetService<ISortedTop>();
		}
	}
}
