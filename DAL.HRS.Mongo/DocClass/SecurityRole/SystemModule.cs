using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class SystemModule 
    {
        public string Id => ModuleType.Id;
        public SystemModuleTypeRef ModuleType { get; set; }
        public bool View { get; set; } = false;
        public bool Add { get; set; } = false;
        public bool Modify { get; set; } = false;
        public bool Delete { get; set; } = false;

        public FileOperations FileOperations { get; set; } = new FileOperations();

        public SystemModule AllAccess()
        {
            View = true; Add = true; Modify = true; Delete = true;
            FileOperations.AllAccess();
            return this;
        }

        public SystemModule ReadOnly()
        {
            View = true; Add = false; Modify = false; Delete = false;
            FileOperations.ReadOnly();
            return this;
        }

        public SystemModule ReadWrite()
        {
            View = true;
            Modify = true;
            Delete = false;
            Add = false;
            FileOperations.ReadWrite();
            return this;
        }

        public SystemModule ReadWriteDelete()
        {
            View = true;
            Modify = true;
            Delete = true;
            Add = false;
            FileOperations.ReadWriteDelete();
            return this;
        }



        public SystemModule()
        {
        }

        public SystemModule(SystemModuleTypeRef moduleType)
        {
            ModuleType = moduleType;
            ReadOnly();
        }

        public static SystemModule Create(SystemModuleTypeRef moduleType)
        {
            var result = new SystemModule(moduleType);
            return result;
        }

        //public static SystemModule CreateOrClone(SystemModuleTypeRef moduleType)
        //{
        //    var rep = new RepositoryBase<SystemModule>();
        //    var module = rep.AsQueryable().FirstOrDefault(x => x.ModuleType.EmployeeId == moduleType.EmployeeId);

        //    if (module == null)
        //    {
        //        module = new SystemModule()
        //        {
        //            ModuleType = moduleType,
        //        };
        //        return rep.Upsert(module);
        //    }
        //    var result = Clone(module);
        //    return rep.Upsert(result);
        //}


        //public static SystemModule Clone(SystemModule systemModule)
        //{
        //    return new SystemModule()
        //    {
        //        ModuleType = systemModule.ModuleType,
        //        CanViewAllEmployees = systemModule.CanViewAllEmployees,
        //        Add = systemModule.Add,
        //        Delete = systemModule.Delete,
        //        Modify = systemModule.Modify,
        //        View = systemModule.View,
        //        GrantedAppPermissions = systemModule.GrantedAppPermissions,
        //        RevokedAppPermissions = systemModule.RevokedAppPermissions,
        //    };
        //}

    }
}