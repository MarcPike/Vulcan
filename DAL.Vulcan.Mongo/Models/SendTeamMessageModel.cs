using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Messages;

namespace Vulcan.WebApi2.Models
{
    public class SendTeamMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
