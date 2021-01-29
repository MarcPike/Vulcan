using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Hse;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class BbsTaskModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BbsTaskSubCategoryRef> SubCategories { get; set; }
        public bool IsDirty { get; set; } = false;

        public BbsTaskModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public BbsTaskModel(BbsTask t)
        {
            Id = t.Id.ToString();
            Name = t.Name;
            SubCategories = t.SubCategories;
        }

    }
}