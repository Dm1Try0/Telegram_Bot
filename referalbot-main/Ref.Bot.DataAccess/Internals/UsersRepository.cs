using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Internals
{
	internal class UsersRepository : IUsersRepository
	{
		private readonly BotContext_REF _context;

		public UsersRepository(BotContext_REF context)
		{
			_context = context;
		}

		public void Add(Users user)
		{
			_context.Users_REF.Add(user);

			_context.SaveChanges();
		}

		public Users? GetByTelegramId(long telegramId) =>
			_context.Users_REF.FirstOrDefault(u => u.TelegramId == telegramId);

		public void Update(Users user)
		{
			_context.Users_REF.Update(user);
			_context.SaveChanges();
		}

		public Users GetById(long id)
		{
			return _context.Users_REF.FirstOrDefault(u => u.Id == id);
		}

		public Users GetByUsername(string username)
		{
			return _context.Users_REF.FirstOrDefault(u => u.Username == username);
		}

		public List<long> GetUserall()
		{
			var result = _context.Users_REF.Select(x => x.TelegramId).ToList();

			return result;
		}
	}
}
