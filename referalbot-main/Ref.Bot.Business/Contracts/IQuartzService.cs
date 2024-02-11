using Quartz;

namespace Ref.Bot.Business.Contracts
{
	public interface IQuartzService
	{
		Task StartJobs(IScheduler scheduler);
	}
}
