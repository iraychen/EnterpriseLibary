using System;
using Castle.Core.Logging;
using Enterprises.Framework.Plugin.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ILogger = Enterprises.Framework.Plugin.Logging.ILogger;

namespace Enterprises.Test.Logging
{
    [TestClass]
    public class ILoggerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Castle.Core.Logging.ILogger cLogger = new WebLogger();
            ILogger logger = new CastleLogger(cLogger);
            logger.Error(new Exception("测试错误,TestMethod1"),"demo");

        }
    }
}
