using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    [BsonIgnoreExtraElements]
    public class HrsUserRef : ReferenceObject<HrsUser>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public LocationRef Location { get; set; }

        public EntityRef Entity { get; set; }

        public HrsUserRef()
        {
        }

        public HrsUserRef(HrsUser user) : base(user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            FullName = user.FullName;
            Location = user.Location;
            Entity = user.Entity;
        }

        public HrsUser AsHrsUser()
        {
            return this.ToBaseDocument();
        }

    }
}