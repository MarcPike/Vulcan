using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class SecurityRoleTypeRef : ReferenceObject<SecurityRoleType>
    {
        public string Name { get; set; }
        public bool IsHrsRole { get; set; }
        public bool IsHseRole { get; set; }

        public SecurityRoleTypeRef()
        {

        }

        public SecurityRoleTypeRef(SecurityRoleType doc) : base(doc)
        {

        }

        public SecurityRoleType AsSecurityRoleType()
        {
            return ToBaseDocument();
        }

    }
}