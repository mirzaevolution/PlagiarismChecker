using FluentScheduler;
namespace Plagiarism.ServiceRunner.Service
{
    public class CrawlerJob : IJob
    {
        public void Execute()
        {
            CrawlerSingleton.GetEngine().Run();
        }
    }
    public class CrawlerExecutor
    {
        public void Start()
        {
            JobManager.RemoveAllJobs();
            Registry registry = new Registry();
            registry.Schedule<CrawlerJob>().ToRunNow().AndEvery(1).Minutes();
            JobManager.Initialize(registry);
        }
        public void Stop()
        {
            JobManager.Stop();
        }
    }
}
