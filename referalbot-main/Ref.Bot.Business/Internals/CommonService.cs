using Mapster;
using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Dtos;
using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.Business.Internals
{
	internal class CommonService : ICommonService
	{
		private readonly ICommonDataRepository _repository;
		public CommonService(ICommonDataRepository repository)
		{
			_repository = repository;
		}
		public CommonDto Creat(string username, string reflink, string botname, string info)
		{
			var refka = new CommonRef()
			{
				Username = username,
				RefLink = reflink,
				BotName = botname,
				Info = info
			};
			_repository.Add(refka);

			var resut = refka.Adapt<CommonDto>();

			return resut;
		}

		public CommonDto? GetByIdInfo(string id)
		{
			var user = _repository.GetByInfo(id);
			var result = user?.Adapt<CommonDto>();

			return result;
		}

		public List<string> GetAllRefLink()
		{
			var result = _repository.GetUserall();
			return result;
		}
		public CommonDto Create(string link, string username, string botname, string info)
		{
			var user = new CommonRef()
			{
				Username = username,
				RefLink = link,
				BotName = botname,
				Info = info
			};
			_repository.Add(user);

			var resut = user.Adapt<CommonDto>();

			return resut;
		}
	}
}
