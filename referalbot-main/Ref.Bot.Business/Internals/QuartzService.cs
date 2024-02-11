using Quartz;
using Ref.Bot.Business.Contracts;
using Ref.Bot.Business.Internals.Quartz.Jobs;

namespace Ref.Bot.Business.Internals
{
	internal class QuartzService : IQuartzService
	{
		public async Task StartJobs(IScheduler scheduler)
		{
			var mymethod = JobBuilder.Create<CheckNewReferals>().Build();

			var orderOperationJobTrigger = TriggerBuilder.Create()
				.WithIdentity("orderOperationJobTrigger", "method")
				.StartNow()
				.WithSimpleSchedule(x => x.WithIntervalInSeconds(180)
				.RepeatForever())
				.Build();

			await scheduler.ScheduleJob(mymethod, orderOperationJobTrigger);
		}
	}
}
