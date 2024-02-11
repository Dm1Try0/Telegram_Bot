using Mapster;
using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Dtos;
using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Entities;

namespace Ref.Bot.Business.Internals
{
	internal class UsersService : IUsersService
	{
		private readonly IUsersRepository _repository;
		public UsersService(IUsersRepository repository)
		{
			_repository = repository;
		}
		public UserDto Create(long telegramId, string username, string firstName)
		{
			var user = new Users()
			{
				TelegramId = telegramId,
				Username = username,
				First = firstName,
				StartingTime = DateTime.UtcNow,
			};
			_repository.Add(user);

			var resut = user.Adapt<UserDto>();

			return resut;
		}
		public async void UpdateAdmin(long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.Status = 4;
			user.Role = "Admin";

			_repository.Update(user);
		}
		public UserDto? Get(long telegramId)
		{
			var user = _repository.GetByTelegramId(telegramId);
			var result = user?.Adapt<UserDto>();

			return result;
		}
		public async void UpdUsername(string username, long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.Username = username;

			_repository.Update(user);
		}
		public async void UpdLastMessage(long telegramid, DateTime message)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.LastMessage = message;

			_repository.Update(user);
		}
		public async void UpdSpam(long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.SpamControl++;

			_repository.Update(user);
		}
		public async void SpamNullable(long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.SpamControl = 0;

			_repository.Update(user);
		}
		public async void UpdName(string name, long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.First = name;

			_repository.Update(user);
		}
		public UserDto? GetByUsername(string username)
		{
			var user = _repository.GetByUsername(username);
			var result = user?.Adapt<UserDto>();

			return result;
		}
		public UserDto? GetById(long userId)
		{
			var user = _repository.GetById(userId);
			var result = user?.Adapt<UserDto>();

			return result;
		}
		public async void IsReferalUpd(long userid)
		{
			var user = _repository.GetById(userid);
			user.IsReferal = true;

			_repository.Update(user);
		}
		public async void HimReferalUpd(long userid, string link)
		{
			var user = _repository.GetById(userid);
			user.HimReferal = link;

			_repository.Update(user);
		}
		public async void StatusUpd(long status, long userId)
		{
			try
			{
				var user = _repository.GetById(userId);
				user.Status = status;

				_repository.Update(user);
			}
			catch
			{

			}
		}

		public List<long> GetAllTelegramId()
		{
			var result = _repository.GetUserall();
			return result;
		}
		public async void AnketAmountUpdate(long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.AnketAmount++;

			_repository.Update(user);
		}
		public async void AnketAmountNullable(long telegramid)
		{
			var user = _repository.GetByTelegramId(telegramid);
			user.AnketAmount = 0;

			_repository.Update(user);
		}
	}
}
