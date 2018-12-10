using Plagiarism.ServiceRunner.Engine;

namespace Plagiarism.ServiceRunner
{
    public class CrawlerSingleton
    {

        private static CrawlerEngine _engine;
        public static CrawlerEngine GetEngine()
        {
            if (_engine == null)
                _engine = new CrawlerEngine();
            return _engine;
        }
    }
}
