using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
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