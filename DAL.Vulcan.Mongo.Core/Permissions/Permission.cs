using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Permissions
{
    public class Permission: BaseDocument
    {
        public string Name { get; set; } = string.Empty;
        public List<CrmUserRef> Users { get; set; } = new List<CrmUserRef>();
    }
}
