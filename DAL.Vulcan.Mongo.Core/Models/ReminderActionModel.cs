using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ReminderActionModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Label { get; set; }
        public string ActionId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecuteOn { get; set; }
        public int Repeat { get; set; }
        public int RepeatMonths { get; set; }
        public int RepeatDays { get; set; }
        public int RepeatHours { get; set; }
        public int RepeatMinutes { get; set; }
        public bool RemindAllTeamMembers { get; set; }

        public ReminderActionModel()
        {
            
        }

        public ReminderActionModel(string application, string userId, string actionId)
        {
            Application = application;
            UserId = userId;
            ActionId = actionId;
        }
    }
}
