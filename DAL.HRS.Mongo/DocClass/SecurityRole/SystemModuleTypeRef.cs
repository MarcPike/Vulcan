using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class SystemModuleTypeRef : ReferenceObject<SystemModuleType>
    {
        public string Name { get; set; }
        public bool IsHrsModule { get; set; }
        public bool IsHseModule { get; set; }

        public SystemModuleTypeRef()
        {

        }

        public SystemModuleTypeRef(SystemModuleType doc) : base(doc)
        {
            Id = doc.Id.ToString();
            Name = doc.Name;
            IsHrsModule = doc.IsHrsModule;
            IsHseModule = doc.IsHseModule;
        }

        public SystemModuleType AsSystemModuleType()
        {
            return ToBaseDocument();
        }

    }
}