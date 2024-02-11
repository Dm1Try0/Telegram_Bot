using Quartz;
using Quartz.Impl;
using Ref.Bot.Api;


public class Program
{
	public static async Task Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
		StdSchedulerFactory factory = new StdSchedulerFactory();
		IScheduler scheduler = await factory.GetScheduler();
		await scheduler.Start();
		await Task.Delay(TimeSpan.FromSeconds(10));
		await scheduler.Shutdown();

	}

	private static IHostBuilder CreateHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}
