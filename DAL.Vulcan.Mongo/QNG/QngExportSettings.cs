using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.QNG
{
    public class QngExportSettings: BaseDocument
    {
        public int SleepHours { get; set; } = 1;
        public int SleepMinutes { get; set; } = 0;
        public bool DontRunOnWeekends { get; set; } = false;

        public TimeSpan GetTimeSpan()
        {
            var now = DateTime.Now;
            var result = now + new TimeSpan(SleepHours, SleepMinutes, 0);

            if (DontRunOnWeekends)
            {
                if (result.DayOfWeek == DayOfWeek.Saturday) result = result.AddDays(2);
                if (result.DayOfWeek == DayOfWeek.Sunday) result = result.AddDays(1);
            }
            return result - now;
        }
    }
}
