using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Dtos;
using Telegram.Bot;

namespace Ref.Bot.Business.Internals
{
	internal class SortedTop : ISortedTop
	{
		private readonly ICommonService _commonService;
		private readonly IUsersService _usersService;
		private readonly ITelegramBotClient _botClient;
		public SortedTop(ICommonService commonService, IUsersService usersService, ITelegramBotClient botClient)
		{
			_commonService = commonService;
			_usersService = usersService;
			_botClient = botClient;
		}



		public async Task SortUsersForTop(long telegramidtomes)
		{
			List<UserDto> sortDtos = new List<UserDto>();
			foreach (var item in _usersService.GetAllTelegramId())
			{
				var getuser = _usersService.Get(item);
				sortDtos.Add(getuser);
			}
			List<UserDto> sortedperusers = sortDtos.OrderByDescending(x => x.AnketAmount).ToList();

			//логика сориторвки

			await _botClient.SendTextMessageAsync(telegramidtomes, $"{char.ConvertFromUtf32(0x1F947)}@{sortedperusers[0].Username}"); //вывод регультатов сортировки
			return;
		}
	}
}
