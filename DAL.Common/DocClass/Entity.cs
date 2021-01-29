using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;

namespace DAL.Common.DocClass
{
    public class Entity : BaseDocument, ICommonDatabaseObject, ISupportLocationNameChanges
    {
        public string Name { get; set; }

        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public EntityRef AsEntityRef()
        {
            return new EntityRef(this);
        }

        public static Entity GetByName(string name)
        {
            return new RepositoryBase<Entity>().AsQueryable().FirstOrDefault(x => x.Name == name);
        }

        public static EntityRef GetRefByName(string name)
        {
            return new RepositoryBase<Entity>().AsQueryable().FirstOrDefault(x => x.Name == name)?.AsEntityRef();
        }

        public static Entity GetById(string id)
        {
            var objectId = ObjectId.Parse(id);
            return new RepositoryBase<Entity>().AsQueryable().First(x => x.Id == objectId);
        }

        public static EntityRef GetRefById(string id)
        {
            var objectId = ObjectId.Parse(id);
            return new CommonRepository<Entity>().AsQueryable().FirstOrDefault(x => x.Id == objectId)?.AsEntityRef();
        }

        public static List<EntityRef> GetEntityRefs()
        {
            return new RepositoryBase<Entity>().AsQueryable().ToList().Select(x => x.AsEntityRef()).ToList();
        }

        public static MongoRawQueryHelper<Entity> Helper = new MongoRawQueryHelper<Entity>();

        public void ChangeOfficeName(string locationId, string newName)
        {
            var locationChange = Locations.FirstOrDefault(x => x.Id == locationId);
            if (locationChange != null)
            {
                locationChange.ChangeOfficeName(locationId, newName, true);
                Helper.Upsert(this);
            }
           
        }

    }
}
