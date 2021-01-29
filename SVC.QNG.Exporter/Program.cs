using System;
using System.IO;
using System.Reflection;
using DAL.Vulcan.Mongo.Base.Context;
using log4net;
using log4net.Config;
using Topshelf;

namespace SVC.QNG.Exporter
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(QngExportService));

        static void Main(string[] args)
        {

            EnvironmentSettings.CrmProduction();
            Log.Info("Entered Main => HostFactory() initialization code");
            HostFactory.Run(x =>
                {
                    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    XmlConfigurator.Configure(new FileInfo(Path.Combine(assemblyFolder, "log4net.config")));

                    x.Service<QngExportService>();
                    x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(60)));
                    x.SetServiceName("SVC.QNG.Exporter");
                    x.StartAutomatically();
                    x.UseLog4Net();
                }
            );
            Log.Info("Completed Main => HostFactory() initialization code");

        }
    }
}
