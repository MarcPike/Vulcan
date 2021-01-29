using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.DocClass.QueueSchedule
{
    public enum ScheduledEventType
    {
        Reminder,
        Email,
        Phone,
        Meeting,
        Task,
        Event,
        Action
    }
}
