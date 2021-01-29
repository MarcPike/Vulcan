using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhHazardTypeRef : ReferenceObject<CoshhHazardType>
    {
        public string HazardType { get; set; }

        public CoshhHazardTypeRef()
        {
        }

        public CoshhHazardTypeRef(CoshhHazardType type) : base(type)
        {
            HazardType = type.HazardType;
        }

        public CoshhHazardType AsCoshhHazardType()
        {
            return this.ToBaseDocument();
        }
    }
}