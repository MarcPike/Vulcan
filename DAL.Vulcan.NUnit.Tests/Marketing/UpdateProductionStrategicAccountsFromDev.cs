using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Marketing;
using DAL.Marketing.Docs;
using DAL.Marketing.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Test;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Marketing
{
    [TestFixture]
    public class UpdateProductionStrategicAccountsFromDev
    {
        
        public List<MarketingAccount> StrategicAccountsFromDevelopment { get; set; } = new List<MarketingAccount>();
        public List<MarketingAccount> StrategicAccountsCreatedForProd { get; set; } = new List<MarketingAccount>();
        public List<Mongo.DocClass.Companies.Company> StrategicCompaniesDevelopment { get; set; } = new List<Mongo.DocClass.Companies.Company>();

        private RepositoryBase<Mongo.DocClass.Companies.Company> _repCompany;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            StrategicAccountsFromDevelopment = new RepositoryBase<MarketingAccount>().AsQueryable()
                .Where(x => x.AccountType == MarketingAccountType.Strategic).ToList();

            StrategicCompaniesDevelopment = QuoteFetcher.GetAllStrategicCompanies();

            //_helperMarketing.G

            

            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void AddAccountsToProduction()
        {
            var repMarketingAccount = new RepositoryBase<MarketingAccount>();
            _repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();
            foreach (var marketingAccount in StrategicAccountsFromDevelopment)
            {
                var newMarketingAccount = new MarketingAccount()
                {
                    AccountType = MarketingAccountType.Strategic,
                    Name = marketingAccount.Name,
                    MarketingAccountFolder = ConvertMarketingAccountFolder(marketingAccount.MarketingAccountFolder)
                };
                Console.WriteLine(ObjectDumper.Dump(newMarketingAccount));

                StrategicAccountsCreatedForProd.Add(newMarketingAccount);
            }

            foreach (var marketingAccount in StrategicAccountsCreatedForProd)
            {
                repMarketingAccount.Upsert(marketingAccount);
            }

        }

        private MarketingAccountFolder ConvertMarketingAccountFolder(MarketingAccountFolder marketingAccountFolder)
        {
            for (int i = 0; i < marketingAccountFolder.Companies.Count-1; i++)
            {
                var newCompanyRef = ConvertCompanyRef(marketingAccountFolder.Companies[i]);
                if (newCompanyRef == null)
                {
                    Console.WriteLine($"Missing Company {marketingAccountFolder.Companies[i].CodePlusName}");
                    continue;
                }

                marketingAccountFolder.Companies[i] = newCompanyRef;

            }

            for (int i = 0; i < marketingAccountFolder.Children.Count-1; i++)
            {
                marketingAccountFolder.Children[i] = ConvertMarketingAccountFolder(marketingAccountFolder.Children[i]);
            }

            return marketingAccountFolder;
        }

        private CompanyRef ConvertCompanyRef(CompanyRef companyRef)
        {

            var devCompany = StrategicCompaniesDevelopment.Single(x => x.Id.ToString() == companyRef.Id);

            var coid = devCompany.Location.GetCoid();

            var companiesWithSameSqlId = _repCompany.AsQueryable().Where(x => x.SqlId == companyRef.SqlId).ToList();

            var result = companiesWithSameSqlId.Single(x => x.Location.GetCoid() == coid);

            if (result != null) return result.AsCompanyRef();

            return null;
        }


    }
}
