using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Reports;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperCompany : HelperBase, IHelperCompany
    {
        private readonly RepositoryBase<Company> _repCompany = new RepositoryBase<Company>();
        private readonly RepositoryBase<CompanyGroup> _repCompanyGroup = new RepositoryBase<CompanyGroup>();

        public List<ReportCompanyQuoteHistoryDetail> GetReportQuoteHistory(Company company, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var reportCompanyQuoteHistoryLoader = new ReportCompanyQuoteHistoryDataLoader();
            reportCompanyQuoteHistoryLoader.LoadCompanyHistoryForDateRange(company, fromDate, toDate);

            var report = new ReportCompanyQuoteHistory(reportCompanyQuoteHistoryLoader, displayCurrency);

            return report.GetReportDataFromLoader();
        }

        public CompanyRef GetCompanyRef(string companyId)
        {
            if (companyId == String.Empty) return null;

            var id = ObjectId.Parse(companyId);
            var company = _repCompany.AsQueryable().SingleOrDefault(x => x.Id == id);
            if (company == null) throw new Exception("No Company found");
            return company.AsCompanyRef();
        }

        public CompanyGroupRef GetCompanyGroupRef(string companyGroupId)
        {
            var id = ObjectId.Parse(companyGroupId);
            var companyGroup = _repCompanyGroup.AsQueryable().SingleOrDefault(x => x.Id == id);
            if (companyGroup == null) throw new Exception("No CompanyGroup found");
            return companyGroup.AsCompanyGroupRef();
        }

        public List<CompanyRef> GetCompaniesForBranch(string branch)
        {
            var rep = new RepositoryBase<Company>();
            var companies = rep.AsQueryable().Where(x => x.Location.Branch == branch).ToList();
            return companies.Select(x => new CompanyRef(x)).OrderBy(x => x.ShortName).ToList();
        }

        public List<string> GetUniqueCompanyBranches()
        {
            var rep = new RepositoryBase<Company>();
            var branches = rep.AsQueryable().Select(x => x.Location.Branch).Distinct().ToList();
            return branches.OrderBy(x => x).ToList();
        }

        public List<CompanyRef> GetCompaniesForUser(CrmUser crmUser)
        {
            var team = crmUser.ViewConfig.Team.AsTeam();
            return team.Companies;
            //return GetUniqueCompaniesForTeam(team.CompanyGroups, team.Companies);
        }

        private List<CompanyRef> GetUniqueCompaniesForTeam(List<CompanyGroupRef> companyGroups, List<CompanyRef> companyRefs)
        {
            var result = new List<CompanyRef>();

            foreach (var companyGroupRef in companyGroups)
            {
                var companyGroup = companyGroupRef.AsCompanyGroup();
                var companies = companyGroup.GetAllCompanies();
                foreach (var companyRef in companies)
                {
                    if (result.All(x => x.Id != companyRef.Id))
                    {
                        result.Add(companyRef);
                    }
                }
            }
            foreach (var companyRef in companyRefs)
            {
                if (result.All(x => x.Id != companyRef.Id))
                {
                    result.Add(companyRef);
                }
            }

            return result;
        }

        public (CompanyRef CompanyRef, Address PrimaryAddress, List<Address> OtherAddresses) GetAllCompanyInfoFromIMetal(string coid, string companyId)
        {
            var rep = new RepositoryBase<Company>();
            var company = rep.Find(companyId);
            if (company == null) throw new Exception("Company not found");
            var companyRef = company.AsCompanyRef();

            var query = new QueryCompany(coid) { Id = companyRef.SqlId };
            var companySearchResult = query.Execute().FirstOrDefault();
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            var imetalPrimaryAddress = companySearchResult.PrimaryAddress;
            var primaryAddress = ConvertAddress(imetalPrimaryAddress, AddressType.Primary);

            var otherAddresses = companySearchResult.Addresses.Select(x => ConvertAddress(x, AddressType.Other))
                .ToList();



            return (companyRef, primaryAddress, otherAddresses);

            Address ConvertAddress(CompanyAddressModel iMetalAddress, AddressType type)
            {
                var newAddress = new Address()
                {
                    Id = Guid.NewGuid(),
                    Country = iMetalAddress.CountryName,
                    AddressLine1 = iMetalAddress.Address,
                    City = iMetalAddress.Town,
                    StateProvince = iMetalAddress.County,
                    County = iMetalAddress.County,
                    PostalCode = iMetalAddress.PostCode,
                    Type = type,
                    AddressLine2 = String.Empty,
                    ExternalCode = iMetalAddress.Code,
                    ExternalStatus = iMetalAddress.Status,
                    ExternalSqlId = iMetalAddress.SqlId,
                    ExternalExists = true
                };
                return newAddress;
            }


        }
    }
}