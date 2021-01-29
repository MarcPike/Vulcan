using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    [BsonIgnoreExtraElements]

    public class ProspectRef : ReferenceObject<Prospect>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Branch { get; set; }

        public ProspectRef()
        {
            
        }

        public ProspectRef(Prospect prospect) : base(prospect)
        {
            
        }

        public Prospect AsProspect()
        {
            return ToBaseDocument();
        }
    }
}