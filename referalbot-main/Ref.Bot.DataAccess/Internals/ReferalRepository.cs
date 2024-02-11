using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Internals
{
	internal class ReferalRepository : IReferalRepository
	{
		private readonly BotContext_REF _context;

		public ReferalRepository(BotContext_REF context)
		{
			_context = context;
		}

		public void Add(Referals info)
		{
			_context.Referals_REF.Add(info);

			_context.SaveChanges();
		}
		public void Update(Referals info)
		{
			_context.Referals_REF.Update(info);
			_context.SaveChanges();
		}
		public Referals GetById(long id)
		{
			return _context.Referals_REF.FirstOrDefault(u => u.Id == id);
		}
		public Referals? GetByTelegramId(long telegramId) =>
		   _context.Referals_REF.FirstOrDefault(u => u.TgId == telegramId);
		public List<int> GetUserall()
		{
			var result = _context.Referals_REF.Select(x => x.Id).ToList();

			return result;
		}
		public void Delete(Referals user)
		{
			_context.Referals_REF.Remove(user);

			_context.SaveChanges();
		}
	}
}
