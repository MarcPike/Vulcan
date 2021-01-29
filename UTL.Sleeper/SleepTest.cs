using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UTL.Sleeper
{
    [TestFixture]
    public class SleepTest
    {
        [Test]
        public void Test()
        {

            var tenSecondsFromNow = DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 0, 0, 3));
            var fiveSeconds = new TimeSpan(0, 0, 0, 5);
            using (var sleeper = new Sleeper())
            {
                sleeper.ExecuteAt = tenSecondsFromNow;
                //sleeper.OnExecute += DoingSomethingNormal;
                //sleeper.OnExecute += DoingSomethingWithEventArgsContinueSetToFalseOnThirdExecution;
                sleeper.OnExecute += DoingSomethingWithForcedExceptionOnThirdIteration;
                sleeper.RepeatEvery = fiveSeconds;
                sleeper.ExecuteNumberOfTimes = 5;
                sleeper.IgnoreExceptions = true;
                sleeper.Start();
            }
        }

        private void DoingSomethingNormal(object sender, SleepEventArgs args)
        {
            var sleeper = sender as Sleeper;
            if (sleeper == null) return;
            Console.WriteLine($"Doing something @ {DateTime.Now}: {sleeper.OnExecution} of {sleeper.ExecuteNumberOfTimes}");
        }

        private void DoingSomethingWithForcedExceptionOnThirdIteration(object sender, SleepEventArgs args)
        {
            var sleeper = sender as Sleeper;
            if (sleeper == null) return;
            Console.WriteLine($"Doing something @ {DateTime.Now}: {sleeper.OnExecution} of {sleeper.ExecuteNumberOfTimes}");
            if (sleeper.OnExecution == 3)
            {
                throw new Exception("I am the monkey wrench!");
            }
        }

        private void DoingSomethingWithEventArgsContinueSetToFalseOnThirdExecution(object sender, SleepEventArgs args)
        {
            var sleeper = sender as Sleeper;
            if (sleeper == null) return;
            Console.WriteLine($"Doing something @ {DateTime.Now}: {sleeper.OnExecution} of {sleeper.ExecuteNumberOfTimes}");
            if (sleeper.OnExecution == 3)
            {
                args.Continue = false;
            }
        }

    }

}
