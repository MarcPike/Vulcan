using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProductManufacturer : BaseDocument
    {
        public int OldHrsId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public LocationRef Location { get; set; }

        public CoshhProductManufacturerRef AsCoshhProductManufacturerRef()
        {
            return new CoshhProductManufacturerRef(this);
        }
    }
}