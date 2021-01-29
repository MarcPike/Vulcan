using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
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