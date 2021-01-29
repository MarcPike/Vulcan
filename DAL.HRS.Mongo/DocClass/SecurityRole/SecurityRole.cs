using DAL.Vulcan.Mongo.Base.DocClass;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class SecurityRole : BaseDocument
    {

        public SecurityRoleTypeRef RoleType { get; set; }
        public List<SystemModule> Modules { get; set; } = new List<SystemModule>();

        public bool DirectReportsOnly { get; set; } = true;

        public SystemModule GetModuleFor(SystemModuleTypeRef moduleTypeRef)
        {
            return Modules.SingleOrDefault(x => x.ModuleType.Id == moduleTypeRef.Id);
        }

        public static MongoRawQueryHelper<SecurityRole> Helper = new MongoRawQueryHelper<SecurityRole>();

        public SecurityRole()
        {
        }

        //public SecurityRole Clone()
        //{
        //    SecurityRole copy = (SecurityRole) this.MemberwiseClone();
        //    copy.EmployeeId = ObjectId.GenerateNewId();
        //    copy.CreateDateTime = DateTime.Now;
        //    copy.IsBaseRole = false;
        //    var clonedModules = new List<SystemModule>();
        //    foreach (var module in copy.Modules)
        //    {
        //        clonedModules.Add(SystemModule.Clone(module)); 
        //    }

        //    copy.Modules = clonedModules;
        //    return copy;
        //}

    }
}
