using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Logger;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Logging
{
    [TestFixture]
    public class LogTesting
    {
        [Test]
        public void BasicTest()
        {
            var logger = new VulcanLogger();
            logger.Log(GetType().Name, MethodBase.GetCurrentMethod().Name,new Exception("This is a test"),true);
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            logger.Log(GetType().Name, MethodBase.GetCurrentMethod().Name, new Exception("This is a test"), true);

        }
    }
}
