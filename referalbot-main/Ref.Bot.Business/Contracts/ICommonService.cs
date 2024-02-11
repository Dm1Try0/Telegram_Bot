using Ref.Bot.Business.Dtos;

namespace Ref.Bot.Business.Contracts
{
	public interface ICommonService
	{
		CommonDto Creat(string username, string reflink, string botname, string info);
		CommonDto? GetByIdInfo(string id);
		CommonDto Create(string link, string username, string botname, string info);
		List<string> GetAllRefLink();
	}
}
