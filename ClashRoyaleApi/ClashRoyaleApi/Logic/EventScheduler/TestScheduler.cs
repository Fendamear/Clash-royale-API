using Quartz;

namespace ClashRoyaleApi.Logic.EventScheduler
{
    public class TestScheduler : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            int param1 = context.JobDetail.JobDataMap.GetInt("param1");

            //code execution here

            return Task.CompletedTask;
        }
    }
}
