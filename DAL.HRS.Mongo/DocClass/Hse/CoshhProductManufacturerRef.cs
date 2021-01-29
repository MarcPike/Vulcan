using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProductManufacturerRef : ReferenceObject<CoshhProductManufacturer>
    {
        public string Name { get; set; }

        public CoshhProductManufacturerRef()
        {
        }

        public CoshhProductManufacturerRef(CoshhProductManufacturer man) : base(man)
        {
            Name = man.Name;
        }

        public CoshhProductManufacturer AsCoshhProductManufacturer()
        {
            return ToBaseDocument();
        }
    }
}