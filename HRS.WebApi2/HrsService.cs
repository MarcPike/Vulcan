using System.IO;
using System.Threading;
using DAL.Vulcan.Mongo.Base.Context;
using Microsoft.AspNetCore.Hosting;

namespace HRS.WebApi2
{
    public class HrsService
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public async void Start()
        {
            EnvironmentSettings.HrsQualityControl();
            EnvironmentSettings.RunningLocal = false;


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(EnvironmentSettings.GetBaseAddress())
                .Build();

            await host.RunAsync();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }
    }
}