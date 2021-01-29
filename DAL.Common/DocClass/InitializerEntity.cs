using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.Common.DocClass
{
    public static class InitializerEntity
    {
        public static void Initialize()
        {
            var queryHelper = new CommonMongoRawQueryHelper<Entity>();
            var howco = queryHelper.Find(queryHelper.FilterBuilder.Where(x => x.Name == "Howco")).FirstOrDefault();
            if (howco == null)
            {
                howco = new Entity()
                {
                    Name = "Howco"
                };
                queryHelper.Upsert(howco);
            }
            var edgen = queryHelper.Find(queryHelper.FilterBuilder.Where(x => x.Name == "Edgen Murray")).FirstOrDefault();
            if (edgen == null)
            {
                edgen = new Entity()
                {
                    Name = "Edgen Murray"
                };
                queryHelper.Upsert(edgen);
            }
            var unknown = queryHelper.Find(queryHelper.FilterBuilder.Where(x => x.Name == "<unknown>")).FirstOrDefault();
            if (unknown == null)
            {
                unknown = new Entity()
                {
                    Name = "<unknown>"
                };
                queryHelper.Upsert(unknown);
            }

        }

        public static void UpdateLocationList(Entity e)
        {
            var locationQueryHelper = new CommonMongoRawQueryHelper<Location>();
            var filter = locationQueryHelper.FilterBuilder.Where(x => x.Entity.Id == e.ToString());
            var myLocations = locationQueryHelper.Find(filter).ToList();
            var changesMade = false;
            foreach (var location in myLocations)
            {
                if (e.Locations.All(x => x.Id != location.Id.ToString()))
                {
                    e.Locations.Add(location.AsLocationRef());
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                var queryHelper = new CommonMongoRawQueryHelper<Entity>();
                queryHelper.Upsert(e);
            }

        }

    }
}