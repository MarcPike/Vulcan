using System;
using System.IO;
using System.Reflection;
using DAL.iMetal.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.Context;
using log4net;
using log4net.Config;
using Topshelf;

namespace SVC.iMetal.CacheBuilder
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheBuilderService));

        static void Main(string[] args)
        {
            EnvironmentSettings.CrmProduction();
            ConnectionFactory.Initialize();

            log.Info("Entered Main => HostFactory() initialization code");
            HostFactory.Run(x =>
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    XmlConfigurator.Configure(new FileInfo(Path.Combine(assemblyFolder, "log4net.config")));
                    //var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
                    //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                    x.Service<CacheBuilderService>();
                    x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(60)));
                    x.SetServiceName("SVC.iMetal.CacheBuilder");
                    x.StartAutomatically();
                    x.UseLog4Net();
                }
            );
            log.Info("Completed Main => HostFactory() initialization code");
        }
    }
}
