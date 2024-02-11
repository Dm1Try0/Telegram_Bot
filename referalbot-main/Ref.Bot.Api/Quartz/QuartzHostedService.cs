using Quartz;
using Ref.Bot.Business.Contracts;

namespace Ref.Bot.Api.Quartz
{
	public class QuartzHostedService : IHostedService
	{
		private readonly IScheduler _scheduler;
		private readonly IQuartzService _quartzService;
		public QuartzHostedService(IScheduler scheduler,
								   IQuartzService quartzService)
		{
			_scheduler = scheduler;
			_quartzService = quartzService;
		}

		public async Task StartAsync(CancellationToken ct)
		{
			await ScheduleJobs();
			await _scheduler.Start(ct);
		}

		public Task StopAsync(CancellationToken ct)
		{
			return _scheduler.Shutdown(ct);
		}

		private async Task ScheduleJobs()
		{
			await _quartzService.StartJobs(_scheduler);
		}
	}
}
