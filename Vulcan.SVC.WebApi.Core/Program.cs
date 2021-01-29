using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topshelf;
using Vulcan.Svc.WebApi.Core;

namespace Vulcan.Svc.WebApi.Core
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
                x.Service<WebApiService>(s =>
                {
                    s.ConstructUsing(name => new WebApiService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan WebApi Service");
                x.SetDisplayName("Vulcan.SVC.WebApi.Core");
                x.SetServiceName("Vulcan.SVC.WebApi.Core");
            });

        }
    }
}
