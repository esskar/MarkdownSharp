using System.Diagnostics;
using NUnit.Framework;

namespace MarkdownSharpTests
{
    public class BaseTest
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(BaseTest).FullName);

        static BaseTest()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Debug("Logging configured");
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            _log.InfoFormat("{0} - Tests starting", GetType().Name);
        }

        [TestFixtureTearDown, DebuggerStepThrough]
        public void TearDown()
        {
            _log.InfoFormat("{0} - Tests complete", GetType().Name);
        }
    }
}