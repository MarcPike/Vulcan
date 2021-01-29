using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    public class HrsSecurityGroup: BaseDocument
    {
        public string Name { get; set; } = string.Empty;
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public List<HrsUserRef> Admins { get; set; } = new List<HrsUserRef>();
        public List<HrsUserRef> Users { get; set; } = new List<HrsUserRef>();
        public List<AppPermissionRef> Permissions { get; set; } = new List<AppPermissionRef>();
    }
}