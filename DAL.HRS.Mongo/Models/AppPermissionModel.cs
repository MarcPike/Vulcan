using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;

namespace DAL.HRS.Mongo.Models
{
    public class AppPermissionModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }


        public AppPermissionModel(AppPermission perm)
        {
            Id = perm.Id.ToString();
            Label = perm.Label;
            Description = perm.Description;
        }
    }
}
