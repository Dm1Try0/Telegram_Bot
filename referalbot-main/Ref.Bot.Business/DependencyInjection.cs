using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Internals;
using Ref.Bot.DataAccess;
using Telegram.Bot;

namespace Ref.Bot.Business
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
		{
			var telegramBotToken = configuration.GetSection("telegramToken").Value;
			return services.AddDataAccessLayer(configuration)
						   .AddTransient<ITelegramBotClient>(x => new TelegramBotClient(telegramBotToken))
						   .AddTransient<ITelegramService, TelegramService>()
						   .AddTransient<IUsersService, UsersService>()
						   .AddTransient<IQuartzService, QuartzService>()
						   .AddTransient<IReferalsQuartz, ReferalsQuartz>()
						   .AddTransient<ICommonService, CommonService>()
						   .AddTransient<ISortedTop, SortedTop>();
		}
		public static IServiceProvider InitialBusinessLayer(this IServiceProvider provider)
		{
			provider.InitialDataAccessLayer();
			var telegramService = provider.GetService<ITelegramService>();
			telegramService?.StartReceiving(provider);

			return provider;
		}
	}
}
