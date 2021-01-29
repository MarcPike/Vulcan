using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class JobTitleRef : ReferenceObject<JobTitle>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public JobTitleRef()
        {
        }

        public JobTitleRef(JobTitle doc) : base(doc)
        {
            Name = doc.Name;
            IsActive = doc.IsActive;
        }

        public JobTitle AsJobTitle()
        {
            return ToBaseDocument();
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }
    }
}