using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Common.DocClass
{
    [BsonIgnoreExtraElements]
    public class LdapUserRef: ReferenceObject<LdapUser>, ISupportLocationNameChangesNested
    {

        public string ActiveDirectoryId { get; set; }
        public string NetworkId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string UserName { get; set; }

        public LocationRef Location { get; set; }


        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public LdapUser AsLdapUser()
        {
            var id = ObjectId.Parse(Id);
            return new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault(x=>x.Id == id);
        }

        public LdapUserRef()
        {
            
        }

        public LdapUserRef(LdapUser document) : base(document)
        {
            Id = document.Id.ToString();
            NetworkId = document.NetworkId;
            UserName = document.UserName;
            FirstName = document.FirstName;
            LastName = document.LastName;
            MiddleName = document.MiddleName;
            Location = document.Location;
            ActiveDirectoryId = document.ActiveDirectoryId;
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = Location.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}