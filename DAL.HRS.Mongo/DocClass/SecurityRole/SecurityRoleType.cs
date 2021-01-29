using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    
    public class SecurityRoleType: BaseDocument
    {
        public static MongoRawQueryHelper<SecurityRoleType> Helper = new MongoRawQueryHelper<SecurityRoleType>();
        public string Name { get; set; }
        
        public bool IsHrsRole { get; set; }
        public bool IsHseRole { get; set; }

        public SecurityRoleTypeRef AsSecurityRoleTypeRef()
        {
            

            return new SecurityRoleTypeRef(this);
        }
    }
}
