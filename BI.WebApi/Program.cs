using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topshelf;

namespace BI.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {


            //EnvironmentSettings.CurrentEnvironment = Environment.Development;
            //EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            //EnvironmentSettings.CurrentEnvironment = Environment.Production;

            HostFactory.Run(x =>
            {
                x.Service<BiService>(s =>
                {
                    s.ConstructUsing(name => new BiService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("BI Web Service)");
                x.SetDisplayName("BI.Web.Service");
                x.SetServiceName("BI.Web.Service");
            });

        }
    }
}
