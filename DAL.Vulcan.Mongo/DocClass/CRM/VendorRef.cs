using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class VendorRef : ReferenceObject<Vendor>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public VendorRef(Vendor vendor) : base(vendor)
        {
            
        }

        public VendorRef()
        {
            
        }

        public Vendor AsVendor()
        {
            return ToBaseDocument();
        }
    }
}