using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.QNG;
using log4net;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using BLL.EMail;
using Topshelf;

namespace SVC.QNG.Exporter
{
    public class QngExportService : ServiceControl
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private QngExportSettings _qngExportSettings;
        private static readonly ILog Log = LogManager.GetLogger(typeof(QngExportService));
        private System.Timers.Timer _startTimer;
        private void Activate()
        {
            Console.WriteLine("Service Activated");
            while (true)
            {
                try
                {
                    if (_cancellationToken.IsCancellationRequested) break;

                    Log.Info($"Beginning export process...");


                    var quoteExportWorker = new QuoteExportWorker();
                    quoteExportWorker.Execute();

                    var repSettings = new RepositoryBase<QngExportSettings>();
                    _qngExportSettings = repSettings.AsQueryable().FirstOrDefault();
                    if (_qngExportSettings == null)
                    {
                        _qngExportSettings = new QngExportSettings();
                        repSettings.Upsert(_qngExportSettings);
                    }

                    var waitSpan = _qngExportSettings.GetTimeSpan();
                    var waitUntil = DateTime.Now + waitSpan;

                    Log.Info($"Export Completed, waiting until {waitUntil} for next iteration.");

                    Log.Info($"Export completed.");
                    Log.Info("----------------------------------------------------------");
                    Log.Info($"Sleeping until : {waitUntil}");

                    Thread.Sleep(waitSpan);

                }
                catch (Exception ex)
                {
                    var emailRecipients = new List<string>()
                    {
                        "Isidro.Gallegos@howcogroup.com",
                        "Marc.Pike@howcogroup.com"
                    };
                    EMailSupport.SendEMailToSupportException("SVC.QNG.Exporter exception:", emailRecipients, ex);
                }
            }
        }

        public bool Start(HostControl hostControl)
        {
            Log.Info("Start() called in Service. Waiting 30 seconds for Windows to catch up...");

            // give service 30 seconds to start
            _startTimer = new System.Timers.Timer { Interval = 30000 };
            _startTimer.Elapsed += OnStartTimerElapsed;
            _startTimer.Enabled = true;
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Info("Stop() called in Service");

            _cancellationToken.Cancel();
            return true;
        }

        private void OnStartTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Log.Info("Activate() called in Service");
            _startTimer.Enabled = false;
            _startTimer.Elapsed -= OnStartTimerElapsed;
            Activate();
        }

    }
}
