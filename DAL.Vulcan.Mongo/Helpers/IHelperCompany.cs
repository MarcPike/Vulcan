using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Reports;

namespace DAL.Vulcan.Mongo.Helpers
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