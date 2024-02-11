using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Contracts
{
	public interface ICommonDataRepository
	{
		void Delete(CommonRef user);
		List<string> GetUserall();
		void Add(CommonRef user);
		void Update(CommonRef user);
		CommonRef GetByInfo(string info);
	}
}
