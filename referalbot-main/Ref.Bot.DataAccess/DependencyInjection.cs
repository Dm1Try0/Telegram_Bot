using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ref.Bot.DataAccess.Contracts;
using Ref.Bot.DataAccess.Internals;

namespace Ref.Bot.DataAccess
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,
														   IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("default");
			return services.AddDbContext<BotContext_REF>(x => x.UseNpgsql(connectionString), ServiceLifetime.Transient)
						   .AddTransient<IUsersRepository, UsersRepository>()
						   .AddTransient<IReferalRepository, ReferalRepository>()
						   .AddTransient<ICommonDataRepository, CommonDataRepository>();
		}

		public static IServiceProvider InitialDataAccessLayer(this IServiceProvider provider)
		{
			var context = provider.GetService<BotContext_REF>();
			try
			{
				context?.Database.Migrate();
			}
			catch (Exception e)
			{ }
			return provider;
		}
	}
}
