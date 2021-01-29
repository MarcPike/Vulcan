using DAL.Vulcan.Mongo.Base.Context;
using Topshelf;
using Vulcan.Email.Service;

namespace Vulcan.Email.Scanner.Service
{
    class Program
    {
        static void Main(string[] args)
        {

            //EnvironmentSettings.CurrentEnvironment = Environment.Development;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;

            HostFactory.Run(x =>                                 
            {
                x.Service<EmailScannerService>(s =>                        
                {
                    s.ConstructUsing(name => new EmailScannerService());     
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Vulcan Email Scanner");        
                x.SetDisplayName("Vulcan.Email.Scanner");                       
                x.SetServiceName("Vulcan.Email.Scanner");                       
            });
        }
    }
}
