using DAL.Vulcan.Mongo.Base.Context;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading;
using Vulcan.IMetal.Context;
using System;
using Environment = System.Environment;

namespace Vulcan.WebApi2
{

    public class VulcanService
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public async void Start()
        {
            EnvironmentSettings.CrmDevelopment();
            EnvironmentSettings.RunningLocal = false;

            SetEnvironmentValues();

            //ContextFactory.IsMssQc = true;

            var host = new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.Limits.MinRequestBodyDataRate = null;

                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(EnvironmentSettings.GetBaseAddress())
                .Build();

            await host.RunAsync();
        }

        private void SetEnvironmentValues()
        {
            var absolutePath = Directory.GetCurrentDirectory();
            var newRelicHome = absolutePath + "\\newrelic";
            var profilerPath = newRelicHome + "\\NewRelic.Profiler.dll";

            Environment.SetEnvironmentVariable("CORECLR_ENABLE_PROFILING", "1");
            Environment.SetEnvironmentVariable("CORECLR_PROFILER", "{36032161-FFC0-4B61-B559-F6C5D41BAE5A}");
            Environment.SetEnvironmentVariable("CORECLR_PROFILER_PATH", profilerPath);
            Environment.SetEnvironmentVariable("CORECLR_NEWRELIC_HOME", newRelicHome);
            Environment.SetEnvironmentVariable("NEW_RELIC_LICENSE_KEY", "e6a3d0224fb3dcf3118ecb66ffd395b8799dNRAL");
            Environment.SetEnvironmentVariable($"NEW_RELIC_APP_NAME", $"Vulcan.WebApi2-{EnvironmentSettings.CurrentEnvironment}");
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }
    }
    
}
