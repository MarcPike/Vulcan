using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UTL.Sleeper
{
    public class SleepEventArgs : EventArgs
    {
        public bool Continue { get; set; } = true;
    }

    public class Sleeper : IDisposable
    {
        public int OnExecution { get; set; } = 0;
        public TimeSpan ExecuteAt { get; set; }
        public TimeSpan RepeatEvery { get; set; } = new TimeSpan(1,0,0,0);
        public bool IgnoreExceptions { get; set; } = true;
        public int ExecuteNumberOfTimes { get; set; } = int.MaxValue;
        

        public delegate void OnExecuteDelegate(object sender, SleepEventArgs args);

        public event OnExecuteDelegate OnExecute;

        public void Start()
        {

            var startDate = DateTime.Now.Date.Add(ExecuteAt);
            if (startDate < DateTime.Now)
            {
                startDate = startDate.AddDays(1);
            }

            var sleepTime = startDate - DateTime.Now;

            Thread.Sleep(sleepTime);

            try
            {
                OnExecution = 1;
                if (!ExecuteAndContinue()) return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Repeat();

        }

        private bool ExecuteAndContinue()
        {
            try
            {
                if (OnExecute != null)
                {
                    var eventArgs = new SleepEventArgs()
                    {
                        Continue = true
                    };
                    OnExecute?.Invoke(this, eventArgs);
                    if (eventArgs.Continue == false)
                    {
                        Console.WriteLine("Sleeper Cancelled");
                        return false;
                    }
                }

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurred OnExecute: {e.Message}");
                return IgnoreExceptions;
            }
        }

        public void Repeat()
        {
            while (OnExecution < ExecuteNumberOfTimes)
            {
                OnExecution++;
                try
                {
                    Thread.Sleep(RepeatEvery);
                    if (!ExecuteAndContinue()) return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }

            }
        }

        public void Dispose()
        {
            OnExecute = null;
        }
    }

}
