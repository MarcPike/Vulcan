using System;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.Common.DocClass
{
    public class LdapUser : BaseDocument, ICommonDatabaseObject
    {
        public string ActiveDirectoryId { get; set; } 
        public string NetworkId { get; set; }
        public string UserName { get; set; }
        public LocationRef Location { get; set; }
        public Person Person { get; set; } = null;

        public string LocationText { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;

        public EntityRef Entity => Location?.Entity;

        public static LdapUser GetByName(string lastName, string firstName)
        {
            return Helper.Find(x => x.LastName == lastName && x.FirstName == firstName).FirstOrDefault();
        }

        public void ParseUserName()
        {
            if (UserName.Contains(","))
            {
                LastName = UserName.Substring(0, UserName.IndexOf(','));

                var indexOf = UserName.IndexOf(',');
                var firstMiddle = UserName.Substring(indexOf + 2, UserName.Length - indexOf - 2);
                if (firstMiddle.Contains(" "))
                {
                    FirstName = firstMiddle.Split(' ')[0];
                    MiddleName = firstMiddle.Split(' ')[1];
                }
                else
                {
                    FirstName = firstMiddle;
                    MiddleName = string.Empty;
                }
            }
            else
            {
                var nameFields = UserName.Split(' ');
                if (nameFields.Length == 3)
                {
                    FirstName = nameFields[0];
                    MiddleName = nameFields[1];
                    LastName = nameFields[2];
                }
                if (nameFields.Length == 2)
                {
                    FirstName = nameFields[0];
                    LastName = nameFields[1];
                }
                if (nameFields.Length == 1)
                {
                    FirstName = nameFields[0];
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}";


        public Person GetDefaultPersonIfMissing()
        {
            if (Person == null)
            {
                Person = new Person(this);

                CommonMongoRawQueryHelper<LdapUser>.SaveToDatabase(this);
            }
            return Person;
        }


        public LdapUserRef AsLdapUserRef()
        {
            return new LdapUserRef(this);
        }

        public static MongoRawQueryHelper<LdapUser> Helper = new MongoRawQueryHelper<LdapUser>();

    }

}
