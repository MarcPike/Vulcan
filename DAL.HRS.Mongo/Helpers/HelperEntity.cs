using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.Vulcan.Mongo.Base.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperEntity : HelperBase
    {
        public List<EntityRef> GetEntities()
        {
            return new RepositoryBase<Entity>().AsQueryable().ToList().Select(x => x.AsEntityRef()).ToList();
        }

        public EntityModel GetEntityModel(string entityId)
        {
            var rep = new RepositoryBase<Entity>();
            var entity = rep.Find(entityId);
            if (entity == null) throw new Exception("Company not found");
            return new EntityModel(entity);
        }
    }
}
