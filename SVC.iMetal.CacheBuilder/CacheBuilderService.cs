using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DAL.iMetal.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule;
using log4net;
using log4net.Core;
using Topshelf;
using Environment = DAL.Vulcan.Mongo.Base.Core.Context.Environment;

namespace SVC.iMetal.CacheBuilder
{
    public class CacheBuilderService : ServiceControl
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CacheBuilderService));

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly CacheBuilderWorker _worker = new CacheBuilderWorker();
        private System.Timers.Timer _startTimer;
        private void Activate()
        {
            Logger.Info("Activate() called in Service");
            TimeSpan sleepTime;
            if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
            {
                sleepTime = new TimeSpan(0,1,0,0);
            }
            else
            {
                //sleepTime = new TimeSpan(0, 0, 30, 0);
                sleepTime = new TimeSpan(0, 1, 0, 0);
            }
            while (!_cancellationToken.IsCancellationRequested)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await _worker.ExecuteAsync(_cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        throw;
                    }
                }).Wait();
                Logger.Info($"Sleeping until {DateTime.Now + sleepTime}");
                Thread.Sleep(sleepTime);
            }

        }

        public bool Start(HostControl hostControl)
        {
            Logger.Info("Start() called in Service");
            Logger.Info("Sleeping for 30 seconds to let Windows catchup...");
            // give service 30 seconds to start
            _startTimer = new System.Timers.Timer {Interval = 30000}; 
            _startTimer.Elapsed += OnStartTimerElapsed;
            _startTimer.Enabled = true;
            return true;
        }

        private void OnStartTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _startTimer.Enabled = false;
            _startTimer.Elapsed -= OnStartTimerElapsed;
            Activate();

        }


        public bool Stop(HostControl hostControl)
        {
            _cancellationToken.Cancel();
            return true;
        }
    }
}
