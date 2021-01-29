using Topshelf;

//using Microsoft.AspNetCore.Server.HttpSys;

namespace HRS.WebApi2
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
                x.Service<HrsService>(s =>
                {
                    s.ConstructUsing(name => new HrsService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan HRS Service)");
                x.SetDisplayName("Vulcan.HRS.Service");
                x.SetServiceName("Vulcan.HRS.Service");
            });

        }
    }
}
