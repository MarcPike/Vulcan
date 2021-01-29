using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Reports;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperCompany
    {
        CompanyRef GetCompanyRef(string companyId);
        CompanyGroupRef GetCompanyGroupRef(string companyGroupId);

        List<CompanyRef> GetCompaniesForBranch(string branch);
        List<string> GetUniqueCompanyBranches();

        List<CompanyRef> GetCompaniesForUser(CrmUser crmUser);

        List<ReportCompanyQuoteHistoryDetail> GetReportQuoteHistory(Company company, DateTime fromDate, DateTime toDate, string displayCurrency);
    }
}