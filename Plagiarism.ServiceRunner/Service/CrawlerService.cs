using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Topshelf;
namespace Plagiarism.ServiceRunner.Service
{

    public class CrawlerService
    {
        public static void ExecuteService()
        {
            try
            {
                TopshelfExitCode returnCode = HostFactory.Run((svc) =>
                {
                    svc.Service<CrawlerExecutor>(obj =>
                    {
                        obj.ConstructUsing(x => new CrawlerExecutor());
                        obj.WhenStarted(x => x.Start());
                        obj.WhenContinued(x => x.Start());
                        obj.WhenPaused(x => x.Stop());
                        obj.WhenStopped(x => x.Stop());
                    });
                    svc.SetDescription("Plagiarism service runner for querying submitted documents");
                    svc.SetDisplayName("Plagiarism Service Runner");
                    svc.SetServiceName("PlagiarismSvcRunner");
                    svc.EnablePauseAndContinue();
                    svc.EnableShutdown();
                    svc.RunAsLocalSystem();
                    svc.OnException((x) =>
                    {
                        File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "plagiarism_log.txt"), $"\n{x.ToString()}");
                    });
                    svc.StartAutomatically();
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
