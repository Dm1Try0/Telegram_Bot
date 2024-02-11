using Quartz;
using Ref.Bot.Business.Contracts;

namespace Ref.Bot.Business.Internals.Quartz.Jobs
{
	internal class CheckNewReferals : IJob
	{
		private readonly IReferalsQuartz _refQuartz;

		public CheckNewReferals(IReferalsQuartz referalsQuartz)
		{
			_refQuartz = referalsQuartz;
		}
		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				await _refQuartz.CheckReferalsMethod();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
