using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class SendUserTeamMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string TeamMessageId { get; set; }
        public string Mood { get; set; }
        public string Message { get; set; }
    }
}
