using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Models
{
    public class HrsSecurityGroupModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public List<HrsUserRef> Admins { get; set; } = new List<HrsUserRef>();
        public List<HrsUserRef> Users { get; set; } = new List<HrsUserRef>();
        public List<AppPermissionRef> Permissions { get; set; } = new List<AppPermissionRef>();


        public HrsSecurityGroupModel()
        {
        }

        public HrsSecurityGroupModel(HrsSecurityGroup group)
        {
            Id = group.Id.ToString();
            Name = group.Name;
            Locations = group.Locations;
            Admins = group.Admins;
            Users = group.Users;
            Permissions = group.Permissions;
        }
    }
}
