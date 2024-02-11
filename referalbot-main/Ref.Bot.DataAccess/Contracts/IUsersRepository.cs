using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Contracts
{
	public interface IUsersRepository
	{
		void Add(Users user);

		Users? GetByTelegramId(long telegramId);
		Users GetByUsername(string username);

		void Update(Users user);

		Users GetById(long id);

		List<long> GetUserall();
	}
}
