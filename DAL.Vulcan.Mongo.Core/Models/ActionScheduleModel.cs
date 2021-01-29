using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ActionScheduleModel : BaseModel
    {
        public string ActionId { get; set; }
        public string ScheduleType { get; set; }
        public int DaysFrom { get; set; } 
        public DateTime EndDate { get; set; }

        public List<string> ActionScheduleTypeList = Enum.GetNames(typeof(ActionScheduleType)).ToList();

        public ActionScheduleModel()
        {
            
        }
        public ActionScheduleModel(string application, string userId, Action action) : base(application, userId)
        {
            var schedule = action.Schedule ?? new ActionSchedule();
            ActionId = action.Id.ToString();
            ScheduleType = schedule.ScheduleType.ToString();
            DaysFrom = schedule.DaysFrom;
            EndDate = schedule.EndDate;
        }
    }

}
