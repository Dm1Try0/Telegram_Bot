namespace Ref.Bot.Business.Contracts
{
	public interface ITelegramService
	{
		void StartReceiving(IServiceProvider serviceProvider);
	}
}
