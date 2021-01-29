using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class CompanySelection
    {
        public string Id {
            get
            {
                var result = string.Empty;
                if (Company != null)
                {
                    result = Company.Id;
                }
                return result;
            }
        }
        public CompanyRef Company { get; set; }
        public bool Selected { get; set; }
    }
}