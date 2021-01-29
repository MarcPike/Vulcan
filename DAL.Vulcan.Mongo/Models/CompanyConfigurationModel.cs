using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Config;

namespace DAL.Vulcan.Mongo.Models
{
    public class CompanyConfigurationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public CompanyRef Company{ get; set; }
        public Configuration Configuration { get; set; }

        public CompanyConfigurationModel()
        {

        }

        public CompanyConfigurationModel(string application, string userId, CompanyConfiguration config)
        {
            Id = config.Id.ToString();
            Company = config.Company;
            Configuration = config.Configuration;
            Application = application;
            UserId = userId;
        }

        public static CompanyConfigurationModel GetForCompany(string application, string userId, CompanyRef company)
        {
            var rep = new RepositoryBase<CompanyConfiguration>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.Company.Id == company.Id);
            if (result == null)
            {
                result = new CompanyConfiguration(company);
                rep.Upsert(result);
            }

            return new CompanyConfigurationModel(application, userId, result);
        }
    }
}