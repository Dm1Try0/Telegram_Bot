using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Internals
{
	internal class CommonDataRepository : ICommonDataRepository
	{
		private readonly BotContext_REF _context;

		public CommonDataRepository(BotContext_REF context)
		{
			_context = context;
		}
		public List<string> GetUserall()
		{
			var result = _context.Common.Select(x => x.RefLink).ToList();

			return result;
		}
		public CommonRef GetByInfo(string info)
		{
			return _context.Common.FirstOrDefault(u => u.Info == info);
		}
		public void Delete(CommonRef user)
		{
			_context.Common.Remove(user);

			_context.SaveChanges();
		}
		public void Add(CommonRef user)
		{
			_context.Common.Add(user);

			_context.SaveChanges();
		}
		public void Update(CommonRef user)
		{
			_context.Common.Update(user);
			_context.SaveChanges();
		}
	}
}
