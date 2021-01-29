using DAL.HRS.Mongo.DocClass.SecurityRole;

namespace DAL.HRS.Mongo.Models
{
    public class SystemModuleModel
    {
        public SystemModuleTypeRef ModuleType { get; set; }
        public string Id { get; set; }
        public bool View { get; set; } = false;
        public bool Add { get; set; } = false;
        public bool Modify { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool DirectReportsOnly { get; set; } = true;
        public FileOperations FileOperations { get; set; } = new FileOperations();

        public SystemModuleModel CanViewAll()
        {
            DirectReportsOnly = false;
            return this;
        }

        public SystemModuleModel AllAccess()
        {
            View = true; Add = true; Modify = true; Delete = true;
            DirectReportsOnly = false;
            FileOperations.AllAccess();
            return this;
        }

        public SystemModuleModel ReadOnly()
        {
            View = true; Add = false; Modify = false; Delete = false;
            FileOperations.ReadOnly();
            return this;
        }

        public SystemModuleModel ReadWrite()
        {
            View = true;
            Modify = true;
            Delete = false;
            Add = false;
            FileOperations.ReadWrite();
            return this;
        }

        public SystemModuleModel ReadWriteDelete()
        {
            View = true;
            Modify = true;
            Delete = true;
            Add = true;
            FileOperations.ReadWriteDelete();
            return this;
        }

        public SystemModuleModel()
        {
        }

        public SystemModuleModel(SystemModuleTypeRef typeRef)
        {
            Id = typeRef.Id;
            ModuleType = typeRef;
            ReadOnly();
        }

        public SystemModuleModel(SystemModule systemModule)
        {
            Id = systemModule.ModuleType.Id;
            ModuleType = systemModule.ModuleType;
            View = systemModule.View;
            Add = systemModule.Add;
            Delete = systemModule.Delete;
            Modify = systemModule.Modify;
            FileOperations = systemModule.FileOperations;
        }


        public SystemModule AsSystemModule()
        {
            var result = new SystemModule
            {
                ModuleType = ModuleType,
                View = View,
                Add = Add,
                Delete = Delete,
                Modify = Modify,
                FileOperations = FileOperations
            };
            return result;
        }
    }
}
