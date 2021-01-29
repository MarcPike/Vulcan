using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hse;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class BbsDepartmentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BbsDepartmentSubCategoryRef> SubCategories { get; set; }
        public bool IsDirty { get; set; } = false;

        public BbsDepartmentModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public BbsDepartmentModel(BbsDepartment d)
        {
            Id = d.Id.ToString();
            Name = d.Name;
            SubCategories = d.SubCategories;
        }

    }
}