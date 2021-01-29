using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace BI.DAL.Mongo.BiUserObjects
{
    public class BiUserRef : ReferenceObject<BiUser>
    {
        public LdapUserRef User { get; set; }
        public Person Person { get; set; }

        public BiUserRef()
        {

        }

        public BiUserRef(BiUser u)
        {
            User = u.User;
            Person = u.Person;
        }

        public BiUser AsBiUser()
        {
            return ToBaseDocument();
        }
    }
}
