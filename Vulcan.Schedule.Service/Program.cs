using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using Topshelf;
using Environment = System.Environment;

namespace Vulcan.Schedule.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            // QC
            EnvironmentSettings.CurrentEnvironment = DAL.Vulcan.Mongo.Base.Context.Environment.QualityControl;
            // Dev
            //EnvironmentSettings.CurrentEnvironment = DAL.Vulcan.Mongo.Base.VulcanContext.Environment.Development;
            //
            HostFactory.Run(x =>                                 
            {
                x.Service<ScheduleService>(s =>                        
                {
                    s.ConstructUsing(name => new ScheduleService());     
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan Schedule Service)");        
                x.SetDisplayName("Vulcan.Scheduler");                       
                x.SetServiceName("Vulcan.Scheduler");
            });
        }
    }
}
