using Ref.Bot.Business.Dtos;

namespace Ref.Bot.Business.Contracts
{
	public interface IUsersService
	{
		UserDto Create(long telegramId, string username, string firstName);
		UserDto? Get(long telegramId);
		void StatusUpd(long status, long userId);
		void IsReferalUpd(long userid);
		List<long> GetAllTelegramId();
		UserDto GetByUsername(string username);
		UserDto GetById(long userId);
		void UpdUsername(string username, long telegramid);
		void UpdName(string name, long telegramid);
		void UpdLastMessage(long telegramid, DateTime message);
		void UpdSpam(long telegramid);
		void SpamNullable(long telegramid);
		void AnketAmountUpdate(long telegramid);
		void AnketAmountNullable(long telegramid);
	}
}
