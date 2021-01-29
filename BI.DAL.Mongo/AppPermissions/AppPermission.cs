using DAL.Vulcan.Mongo.Base.DocClass;

namespace BI.DAL.Mongo.AppPermissions
{
    public class AppPermission: BaseDocument
    {
        public string Label { get; set; }
        public string Description { get; set; }

        public AppPermissionRef AsAppPermissionRef()
        {
            return new AppPermissionRef(this);
        }
    }
}