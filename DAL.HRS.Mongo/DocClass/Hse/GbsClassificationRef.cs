using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class GhsClassificationTypeRef : ReferenceObject<GhsClassificationType>
    {
        public string Name { get; set; }

        public GhsClassificationTypeRef() { }

        public GhsClassificationTypeRef(GhsClassificationType cls) : base(cls)
        {
            Name = cls.Name;
        }

        public GhsClassificationType AsGhsClassificationType()
        {
            return ToBaseDocument();
        }
    }
}