using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class CertificationRef : ReferenceObject<Certification>
    {
        public string Name { get; set; }
        public CertificationRef(Certification c)
        {
            Name = c.Name;
        }

        public Certification AsCertification()
        {
            return ToBaseDocument();
        }
    }
}