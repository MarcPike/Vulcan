using System.Text;
using DAL.Vulcan.Mongo.Base.Context;
using Topshelf;

namespace Vulcan.IMetal.Quote.Export.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            
            EnvironmentSettings.CrmDevelopment();
            //
            HostFactory.Run(x =>
            {
                x.Service<QuoteExportService>(s =>
                {
                    s.ConstructUsing(name => new QuoteExportService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan Quote Exporter Service)");
                x.SetDisplayName("Vulcan.IMetal.Exporter");
                x.SetServiceName("Vulcan.IMetal.Exporter");
            });


        }
    }
}
