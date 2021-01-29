using DAL.Vulcan.Mongo.Base.DocClass;

namespace BI.DAL.Mongo.AppPermissions
{
    public class AppPermissionRef : ReferenceObject<AppPermission>
    {
        public string Label { get; set; }

        public AppPermissionRef(AppPermission permission)
        {

        }


        public AppPermission AsAppPermission()
        {
            return ToBaseDocument();
        }
    }
}