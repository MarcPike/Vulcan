using System;
using Devart.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Results;
using Vulcan.IMetal.ViewFilterObjects;


// Marc's Latest
namespace Vulcan.IMetal.Queries.Companies
{
    public class QueryCompany: QueryBase<CompanySearchResult>
    {
        public string QueryCoid { get; set; }
        public CompanyContext Context;

        public int Id { get; set; } = int.MinValue;
        public CustomersCode Code { get; set; } = new CustomersCode();
        public CustomersName Name { get; set; } = new CustomersName();
        public CustomersShortName ShortName { get; set; } = new CustomersShortName();

        public CustomersTypeCode Type { get; set; } = new CustomersTypeCode();
        public CustomersTypeCodeDescription TypeDescription { get; set; } = new CustomersTypeCodeDescription();
        public CustomersStatus Status { get; set; } = new CustomersStatus();
        public CustomersStatusDescription StatusDescription { get; set; } = new CustomersStatusDescription();

        public CompanySearchResult GetForId(int id)
        {
            Id = id;
            var query = BuildQuery(true);
            query = query.Where(x => x.Id == id);


            return query.Select(x => new CompanySearchResult(Context, Coid, x, true)).FirstOrDefault();

            //return Execute().FirstOrDefault();
        }

        public CompanySearchResult GetForCode(string code)
        {
            Code.Value = code;
            return Execute().FirstOrDefault();
        }

        public CompanySearchResult GetForCodeCustomerOnly(string code)
        {
            Code.Value = code;
            return Execute().FirstOrDefault(x => x.CompanyType == "C");
        }

        public override List<CompanySearchResult> Execute()
        {
            var companies = BuildQuery(true);
            return companies.Select(x => new CompanySearchResult(Context,Coid,x, true)).ToList();
        }

        public override List<CompanySearchResult> ExecuteWithLimit(int numberToTake)
        {
            var companies = BuildQuery(true);
            return companies.Select(x => new CompanySearchResult(Context, Coid, x, true)).Take(numberToTake).ToList();
        }


        public List<CompanySearchResult> ExecuteWithoutAddresses()
        {
            var companies = BuildQuery(false);

            return companies.Select(x => new CompanySearchResult(Context,Coid, x, false)).ToList();
        }

        public CompanyCreditRuleModel GetCreditRuleForCompany(int sqlId)
        {
            var company = Context.Company.FirstOrDefault(x => x.Id == sqlId);
            if (company == null)
            {
                throw new Exception("Company not found");
            }

            var creditRule = Context.CompanyCreditRule.FirstOrDefault(x => x.Id == company.CompanyCreditRulesId);
            if (creditRule == null)
            {
                throw new Exception("No Credit Rule found for this Company");
            }

            return new CompanyCreditRuleModel(creditRule);
        }

        public IQueryable<Company> BuildQuery(bool withAddresses)
        {
            IQueryable<Company> companies;
            if (withAddresses)
            {
                companies = Context.Company
                    .LoadWith(x => x.CompanyTypeCode)
                    .LoadWith(x => x.CompanySubAddress)
                    .LoadWith(x => x.SalesGroup)
                    .LoadWith(x => x.OrderClassification)
                    .LoadWith(x => x.CompanyStatusCode);

            }
            else
            {
                companies = Context.Company;
            }

            if (Id > int.MinValue)
                companies = companies.Where(x => x.Id == Id);

            companies = Code.ApplyFilter(companies);
            companies = Name.ApplyFilter(companies);
            companies = ShortName.ApplyFilter(companies);
            companies = Type.ApplyFilter(companies);
            companies = TypeDescription.ApplyFilter(companies);
            companies = Status.ApplyFilter(companies);
            companies = StatusDescription.ApplyFilter(companies);
            return companies;
        }

        public List<CompanyAddressModel> GetAddressesForId(int id)
        {
            var result = new List<CompanyAddressModel>();
            var companies = Context.Company.
                LoadWith(x => x.CompanyTypeCode).
                LoadWith(x => x.CompanySubAddress);

            companies = companies.Where(x => x.Id == id);

            var company = companies.FirstOrDefault();
            if (company == null) return result;

            result.AddRange(company.CompanySubAddress.Select(x => new CompanyAddressModel(x,Coid)).ToList());
            return result;
        }

        public Term GetTermForId(int id)
        {
            var companies = Context.Company.
                LoadWith(x => x.Term);
            companies = companies.Where(x => x.Id == id);

            var company = companies.FirstOrDefault();

            return company?.Term;

        }

        public QueryCompany(string coid) : base(coid)
        {
            Context = ContextFactory.GetCompanyContextForCoid(coid);
        }

        public List<Contact> GetCompanyContacts(int sqlId)
        {
            return Context.Contact.Where(x => x.CompanyId == sqlId).ToList();

        }

        public List<iMetalFreightTerm> GetFreightTerms(string coid)
        {
            return Context.TransportTypeCode.Where(x => x.Status == "A" && x.Type == "S").
                Select(x => new iMetalFreightTerm(){ Coid =coid, Code = x.Code, Description = x.Description }).ToList();
        }

        public List<iMetalPaymentTerm> GetPaymentTerms(string coid)
        {
            return Context.Term.Where(x => x.Status == "A").
                Select(x => new iMetalPaymentTerm() { Coid = coid, Code = x.Code, Description =  x.Description }).ToList();
        }

        public List<iMetalCompany> GetCompanies(string coid)
        {
            return Context.Company
                .Where(x => x.TypeId != 3)
                .Select(x => new iMetalCompany() 
                { 
                    SqlId = x.Id, 
                    Code = x.Code, 
                    Name = x.Name, 
                    ShortName = x.ShortName, 
                    Address = x.Address, 
                    StatusId = x.StatusId, 
                    Addresses = 
                        x.CompanySubAddress.Select(a => a.Address) }).ToList();
        }

        public iMetalCompanyCurrencyCode GetCompanyCurrencyCode(string companyCode)
        {
            return Context.ExecuteQuery<iMetalCompanyCurrencyCode>($@"
            SELECT
                cc.code AS Currency
            FROM companies c 
                INNER JOIN currency_codes cc ON c.currency_id = cc.id
            WHERE c.Code = '{companyCode}' AND c.type_id = 1 AND c.Status = 'A'").FirstOrDefault();


        }

        public iMetalCompanyCreditRuleResult GetCompanyCreditRuleResult(string companyCode)
        {
            var result = Context.ExecuteQuery<iMetalCompanyCreditRuleResult>($@"
            SELECT
                c.name as CompanyName, 
                cc.code AS Currency,
                ccr.status AS CreditStatus,
                ccr.credit_limit AS CreditLimit,
                ccr.credit_limit_percentage AS CreditLimitPercentage,
                ccr.overdue_days_allowed AS OverdueDaysAllowed,
                ccr.overdue_percentage_allowed AS OverduePercentageAllowed,
                ccr.credit_limit_date AS CreditLimitDate,
                ccr.credit_limit_expiry AS CreditLimitExpiry,
                t.description AS PaymentTerm
            FROM companies c 
                INNER JOIN currency_codes cc ON c.currency_id = cc.id
                INNER JOIN company_credit_rules ccr ON c.company_credit_rules_id = ccr.id
                INNER JOIN terms t ON c.terms_id = t.id
    
            WHERE c.Code = '{companyCode}' AND c.type_id = 1 AND ccr.Status = 'A'").FirstOrDefault();

            return result;
        }

        public List<iMetalInvoice> GetInvoices(string companyCode)
        {
            return Context.ExecuteQuery<iMetalInvoice>($@"
                SELECT 
                    cc.code AS Currency,
                    sh.number AS Number, 
                    si.item_number AS ItemNumber, 
                    sh.customer_order_number AS CustomerOrderNumber, 
                    st.balance_value AS Balance, 
                    si.due_date AS DueDate,
                    DATE_PART('day', si.due_date - NOW()) AS DaysToPay,
                    t.due_days AS DueDays
                FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
                    INNER JOIN companies c ON sh.customer_id=c.id
                    INNER JOIN currency_codes cc ON c.currency_id = cc.id
                    INNER JOIN terms t ON sh.terms_id=t.id
                    INNER JOIN sales_types ON sh.type_id=sales_types.id
                    INNER JOIN sales_totals st ON si.sales_total_id=st.id
                    INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
                WHERE c.code='{companyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD') AND (ssc.code<>'CMP' AND ssc.code<>'DEL')
             ").ToList();
        }

        public iMetalCompanyCreditTotalResults GetCompanyCreditTotalResults(string companyCode)
        {
            return Context.ExecuteQuery<iMetalCompanyCreditTotalResults>($@"
            SELECT
                            SUM(CASE WHEN si.due_date>NOW() THEN st.balance_value ELSE 0 END) AS TotalOpen,
                            SUM(CASE WHEN si.due_date<=NOW() THEN st.balance_value ELSE 0 END) AS TotalDue,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW())<31 THEN st.balance_value ELSE 0 END) AS TotalDue30,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 31 AND 60 THEN st.balance_value ELSE 0 END) AS TotalDue60,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 61 AND 90 THEN st.balance_value ELSE 0 END) AS TotalDue90,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 91 AND 120 THEN st.balance_value ELSE 0 END) AS TotalDue120,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW())>120 THEN st.balance_value ELSE 0 END) AS TotalDueOver120,
                SUM(CASE WHEN ssc.code<>'CMP' AND ssc.code<>'DEL' THEN 1 ELSE 0 END) AS OpenInvoices,
                SUM(CASE ssc.code WHEN 'CMP' THEN 1 ELSE 0 END) AS CompletedInvoices,
                MAX(COALESCE(st.balance_value, 0)) AS LargestInvoice
            FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
            INNER JOIN companies c ON sh.customer_id=c.id
            INNER JOIN terms t ON sh.terms_id=t.id
            INNER JOIN sales_types ON sh.type_id=sales_types.id
            INNER JOIN sales_totals st ON si.sales_total_id=st.id
            INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
            WHERE c.code='{companyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD') AND (ssc.code<>'CMP' AND ssc.code<>'DEL')").Single();

        }

        public iMetalTotalOpenCompletedAndLargestResult GetTotalOpenCompletedAndLargest(string companyCode)
        {
            return Context.ExecuteQuery<iMetalTotalOpenCompletedAndLargestResult>($@"
            SELECT 
                MAX(COALESCE(st.balance_value, 0)) AS LargestInvoice,
                SUM(CASE WHEN ssc.code <> 'CMP' AND ssc.code<>'DEL' THEN 1 ELSE 0 END) AS OpenInvoices,
                SUM(CASE WHEN ssc.code  = 'CMP' THEN 1 ELSE 0 END) AS CompletedInvoices
            FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
                INNER JOIN companies c ON sh.customer_id=c.id
                INNER JOIN sales_types ON sh.type_id=sales_types.id
                INNER JOIN sales_totals st ON si.sales_total_id=st.id
                INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
            WHERE c.code='{companyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD')").Single();

        }
    }
}
