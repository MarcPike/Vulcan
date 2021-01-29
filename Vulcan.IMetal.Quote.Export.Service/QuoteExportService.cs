using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using Vulcan.iMetal.Quote.Export.Repository;

namespace Vulcan.IMetal.Quote.Export.Service
{
    public class QuoteExportService
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly QuoteExportWorker _worker = new QuoteExportWorker();

        public async void Start()
        {
            var rep = new RepositoryBase<QueueSchedule>();

            await Activate();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public async Task Activate()
        {
            await Task.Run(() =>
                {
                    while (!_cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            _worker.LookForQuotesToExport();
                            Thread.Sleep(10000);
                            _worker.CheckQuoteImportStatus();
                            Thread.Sleep(10000);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                }
            );
        }
    }
}