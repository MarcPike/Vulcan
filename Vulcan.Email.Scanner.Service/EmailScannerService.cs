using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Email;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;

namespace Vulcan.Email.Service
{
    public class EmailScannerService
    {
        private const string QueueName = "ScheduledEvents";
        private EmailScanner _scanner;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public async void Start()
        {
            var rep = new RepositoryBase<QueueSchedule>();
            _scanner = new EmailScanner();

            await Activate();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public async Task Activate()
        {
            await Task.Run(async () =>
                {

                    while (true)
                    {
                        try
                        {
                            if (_cancellationToken.IsCancellationRequested) break;

                            var task = Task.Run(() => _scanner.Execute());

                            var result = await task;

                            Console.WriteLine("=======================================================================================");
                            Console.WriteLine($"ExecutedOn: {result.ExecutedOn} Process Time: {result.ProcessTime} Emails found: {result.EmailsAdded.Count}");
                            Console.WriteLine("=======================================================================================");
                            Console.WriteLine("");
                            Thread.Sleep(15000);

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
