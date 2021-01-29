using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Permissions;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class PermissionModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<CrmUserRef> Users { get; set; }

        public PermissionModel() { }

        public PermissionModel(string application, string userId, Permission permission)
        {
            Application = application;
            UserId = userId;
            Id = permission.Id.ToString();
            Name = permission.Name;
            Users = permission.Users;
        }
    }
}
