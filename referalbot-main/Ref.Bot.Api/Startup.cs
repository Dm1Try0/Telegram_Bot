using Quartz.Impl;
using Ref.Bot.Api.Extensions;
using Ref.Bot.Api.Quartz;
using Ref.Bot.Business;
using Ref.Bot.Business.Internals.Quartz;

namespace Ref.Bot.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddBusinessLayer(_configuration);
			var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
			scheduler.JobFactory = new JobFactory(services.BuildServiceProvider());
			services.AddSingleton(scheduler);
			services.AddHostedService<QuartzHostedService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Initial();
		}
	}
}
