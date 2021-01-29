using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.Config
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