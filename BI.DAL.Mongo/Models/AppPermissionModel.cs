using BI.DAL.Mongo.AppPermissions;

namespace BI.DAL.Mongo.Models
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
