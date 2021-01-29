using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class CrmUserRef : ReferenceObject<CrmUser>
    {
        public string UserId { get; set; }
        public string Type { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public CrmUserRef()
        {
        }

        public CrmUserRef(CrmUser user) : base(user)
        {
            UserId = user.User.Id;
            Type = user.UserType.ToString();
            FullName = user.User.GetFullName();
            FirstName = user.User.FirstName;
            LastName = user.User.LastName;
        }

        public CrmUser AsCrmUser()
        {
            return ToBaseDocument();
        }
    }
}