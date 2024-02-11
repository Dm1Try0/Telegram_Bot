using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.DataAccess.Contracts
{
	public interface IReferalRepository
	{
		void Add(Referals info);
		void Update(Referals info);
		Referals GetById(long id);
		List<int> GetUserall();
		void Delete(Referals user);
	}
}
