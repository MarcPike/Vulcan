using DAL.Vulcan.Mongo.Base.Context;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BI.WebApi
{
    public class BiService
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public async void Start()
        {
            EnvironmentSettings.BiDevelopment();
            EnvironmentSettings.RunningLocal = true;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
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
