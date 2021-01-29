using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ViewConfigModel
    {
        public string ViewType { get; set; }
        public ObjectId Id { get; set; }
        public string Label { get; set; }
    }
}
