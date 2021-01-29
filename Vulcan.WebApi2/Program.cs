using DAL.Vulcan.Mongo.Base.Context;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Topshelf;

//using Microsoft.AspNetCore.Server.HttpSys;

namespace Vulcan.WebApi2
{
    public class Program
    {
        // Development - 5000
        // QA User Testing - 5010
        // Production - 5150
        // Local Testing - http://192.168.102.221:5000

        /*
            Username: sa.vulcan
            Password: 4FcJ8Y9SnD
         */

        public static void Main(string[] args)
        {

            
            //EnvironmentSettings.CurrentEnvironment = Environment.Development;
            //EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            //EnvironmentSettings.CurrentEnvironment = Environment.Production;

            HostFactory.Run(x =>
            {
                x.Service<VulcanService>(s =>
                {
                    s.ConstructUsing(name => new VulcanService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan Web Service)");
                x.SetDisplayName("Vulcan.Web.Service");
                x.SetServiceName("Vulcan.Web.Service");
            });

        }
    }
}
