using Ref.Bot.Business;

namespace Ref.Bot.Api.Extensions
{
	public static class StartupExtensions
	{
		public static IApplicationBuilder Initial(this IApplicationBuilder builder)
		{
			builder.ApplicationServices
				   .CreateScope()
				   .ServiceProvider
				   .InitialBusinessLayer();

			return builder;
		}
	}
}
