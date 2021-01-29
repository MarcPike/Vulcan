using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Vulcan.Mongo.DocClass.Config
{
    public class CompanyConfiguration : BaseDocument
    {
        public CompanyRef Company { get; set; }
        public Configuration Configuration { get; set; } = new Configuration();

        public CompanyConfiguration()
        {

        }

        public CompanyConfiguration(CompanyRef company, Configuration configuration)
        {
            Company = company;
            Configuration = configuration;
        }
        public CompanyConfiguration(CompanyRef company)
        {
            Company = company;
        }
    }
}