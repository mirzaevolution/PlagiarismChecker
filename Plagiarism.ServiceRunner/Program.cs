using Plagiarism.ServiceRunner.Service;

namespace Plagiarism.ServiceRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //CrawlerSingleton.GetEngine().Run();
            CrawlerService.ExecuteService();
        }
    }
}
