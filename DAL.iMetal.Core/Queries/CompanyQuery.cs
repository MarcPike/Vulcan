using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Models;
using Npgsql;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.iMetal.Core.Queries
{
    
    public class CompanyQuery : BaseQuery<CompanyQuery>
    {
        public static NpgsqlConnection Connection = null;
        public string Coid { get; set; }
        public int Id { get; set; }
        public string Branch { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string CompanyType { get; set; }
        public string TypeDescription { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupDescription { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public int AddressId { get; set; }

        public List<iMetalCompanyAddressModel> Addresses { get; set; }
        public iMetalCompanyAddressModel PrimaryAddress { get; set; }

        public string Telephone { get; set; }
        public string FastDial { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Categories { get; set; }

        public string ProductsNote { get; set; }

        public string CompetitionNote { get; set; }

        public string AccountsNote { get; set; }

        public string GeneralNote { get; set; }
        public string PopupNotes { get; set; }

        public bool PaymentHold { get; set; }

        public string PaymentTerms { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? Created { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string DefaultSalesGroupCode { get; set; }
        public string OrderClassificationCode { get; set; }
        public string OrderClassificationDescription { get; set; }
        public string CompanyStatusCode { get; set; }
        public string CompanyStatus { get; set; }
        public string CompanyStatusDescription { get; set; }
        public int CreditRulesId { get; set; }

        public async Task<List<iMetalCompanyAddressModel>> GetAllAddresses()
        {
            try
            {
                var sqlQuery = new SqlQuery<iMetalCompanyAddressModel>();
                var result = await sqlQuery.ExecuteQueryAsync(Coid,
                    $@"SELECT 
                      '{Coid}' AS Coid,
                      csa.name AS Name,
                      a.id AS AddressId, 
                      a.address AS Address, 
                      a.town AS Town, 
                      a.county AS County, 
                      a.postcode AS PostCode, 
                      a.status AS Status,
                      cc1.description AS CountryName,
                      csa.code as Code,
                      a.id as SqlId
                      FROM
                        public.company_sub_addresses csa
                      LEFT OUTER JOIN public.addresses a ON csa.address_id = a.id
                      LEFT OUTER JOIN PUBLIC.country_codes cc1 ON a.country_id = cc1.id
                      WHERE csa.company_id = {Id}");

                var addresses = result.ToList();

                if (addresses == null) return null;

                Addresses = addresses;

                return addresses;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<iMetalCompanyAddressModel> GetPrimaryAddress()
        {
            try
            {
                var sqlQuery = new SqlQuery<iMetalCompanyAddressModel>();
                var name = Name;
                if (name.Contains('\''))
                {
                    name = name.Replace( "'", "''");  
                }
                var result = await sqlQuery.ExecuteQueryAsync(Coid,
                    $@"SELECT 
                      '{Coid}' AS Coid,
                      '{name}' AS Name,
                      a.id AS AddressId, 
                      a.address AS Address, 
                      a.town AS Town, 
                      a.county AS County, 
                      a.postcode AS PostCode, 
                      a.status AS Status,
                      cc1.description AS CountryName,
                      'PRIMARY' as Code,
                      a.id as SqlId
                      FROM
                        public.addresses a
                      LEFT OUTER JOIN PUBLIC.country_codes cc1 ON a.country_id = cc1.id
                      WHERE a.id = {AddressId}");

                var addressInfo = result.SingleOrDefault();

                if (addressInfo == null) return null;

                PrimaryAddress = addressInfo;

                return PrimaryAddress;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetBaseSql(string coid)
        {
                return
                    @$"
                    SELECT 
                      '{coid}' as Coid,
                      c.id AS Id, c.name AS Name, c.short_name AS ShortName, c.code AS Code, b.name AS Branch,
                      ind.code AS StdIndustrialClassCode, ind.description AS StdIndustrialClassDescription,
                      cgp.code AS CustomerGroupCode, cgp.description AS CustomerGroupDescription,
                      COALESCE(oc.code,'<unspecified>') AS OrderClassificationCode, COALESCE(oc.description,'<unspecified>') AS OrderClassificationDescription,
                      cc1.code AS CountryCode, cc1.description AS CountryName,
                      csc.code AS CompanyStatusCode, csc.status AS CompanyStatus, csc.description AS CompanyStatusDescription,
                      COALESCE(sg.code,'') AS DefaultSalesGroupCode, t.description AS PaymentTerms,
                      c.telephone AS Telephone,
                      c.fast_dial AS FastDial,
                      c.fax AS Fax,
                      c.email AS Email,
                      c.payment_hold AS PaymentHold,
                      cc.code AS CurrencyCode,
                      cc.name AS CurrencyName,
                      c.status AS Status,
                      c.status_id AS StatusId,
                      ctc.code AS CompanyType,
                      ctc.description AS CompanyTypeDescription,
                      c.address_id AS AddressId,
                      c.company_credit_rules_id AS CreditRulesId
                    FROM public.companies c
                        LEFT OUTER JOIN public.company_type_codes ctc ON c.type_id = ctc.id
                        LEFT OUTER JOIN public.sales_groups sg ON c.sales_group_id = sg.id
                        LEFT OUTER JOIN public.order_classifications oc ON c.default_order_classification_id = oc.id
                        LEFT OUTER JOIN public.addresses a ON c.address_id = a.id
                        LEFT OUTER JOIN PUBLIC.country_codes cc1 ON a.country_id = cc1.id
                        LEFT OUTER JOIN public.company_status_codes csc ON c.status_id = csc.id
                        LEFT OUTER JOIN public.customer_analysis_codes ind ON c.analysis_code_2_id = ind.id
                        LEFT OUTER JOIN public.customer_analysis_codes cgp ON c.analysis_code_1_id = cgp.id
                        LEFT OUTER JOIN public.terms t ON c.terms_id = t.id
                        LEFT OUTER JOIN public.currency_codes cc ON c.currency_id = cc.id
                        LEFT OUTER JOIN public.branches b ON c.branch_id = b.id
                    ";
        }

        public CompanyQuery()
        {

        }


        public static async Task<CompanyQuery> GetForId(string coid, int id, bool withAddresses = false)
        {
            
            var sqlQuery = new SqlQuery<CompanyQuery>();
            var sql = CompanyQuery.GetBaseSql(coid) + $" WHERE c.id = {id}";
            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            var result = queryResult.SingleOrDefault();

            if (result == null) return null;

            await HandleAddress(withAddresses, result);

            return result;
        }

        public static async Task<CompanyQuery> GetForCode(string coid, string code, bool withAddresses = false)
        {
            var sqlQuery = new SqlQuery<CompanyQuery>();
            var sql = CompanyQuery.GetBaseSql(coid) + $" WHERE c.code = '{code}' AND ctc.code ='C'";
            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            var list = queryResult.ToList();

            var result = queryResult.SingleOrDefault();

            if (result == null) return null;

            await HandleAddress(withAddresses, result);
            return result;
        }

        private static async Task HandleAddress(bool withAddresses, CompanyQuery result)
        {
            await result.GetPrimaryAddress();
            if (withAddresses)
            {
                await result.GetAllAddresses();
            }
        }

        private static async Task HandleAddresses(bool withAddresses, List<CompanyQuery> results)
        {
            foreach (var companyQuery in results)
            {
                await companyQuery.GetPrimaryAddress();
                if (withAddresses)
                {
                    await companyQuery.GetAllAddresses();
                }
            }

        }

        public static async Task<List<CompanyQuery>> GetAllCompaniesForCoid(string coid, bool withAddresses = false)
        {
            var sqlQuery = new SqlQuery<CompanyQuery>();
            var sql = CompanyQuery.GetBaseSql(coid);
            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            var result = queryResult.ToList();

            await HandleAddresses(withAddresses, result);
            return result;
        }

        public async Task<iMetalCompanyCreditRuleModel> GetCreditRule()
        {
            var sqlQuery = new SqlQuery<iMetalCompanyCreditRuleModel>();

            var sql = $@"
                SELECT 
                    ccr.status AS Status,
                    ccr.credit_limit AS CreditLimit,
                    ccr.credit_limit_percentage AS CreditLimitPercentage,
                    ccr.overdue_days_allowed AS OverdueDaysAllowed,
                    ccr.overdue_percentage_allowed AS OverduePercentageAllowed,
                    ccr.credit_limit_date AS CreditLimitDate,
                    ccr.credit_limit_expiry AS CreditLimitExpiry
                FROM public.company_credit_rules ccr
                WHERE id = {CreditRulesId}
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(Coid, sql);

            return queryResult.SingleOrDefault();
        }



        public async Task<List<iMetalContactModel>> GetContacts()
        {
            var sqlQuery = new SqlQuery<iMetalContactModel>();

            var sql = $@"
                SELECT
                  c.id as SqlId,
                  c.forename AS FirstName,
                  c.surname AS LastName,
                  c.mobile AS PhoneMobile,
                  c.fax AS PhoneFax,
                  c.fast_dial AS PhoneOffice,
                  c.email AS EmailBusiness,
                  a.address AS Address1,
                  a.county AS StateProvince,
                  a.postcode AS PostalCode,
                  a.town AS City
                FROM public.contacts c 
                INNER JOIN public.addresses a ON c.address_id = a.id
                WHERE c.company_id = {Id}
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(Coid, sql);

            return queryResult.ToList();

        }

        public static async Task<List<iMetalContactModel>> GetCompanyContacts(string coid, int sqlId)
        {
            var sqlQuery = new SqlQuery<iMetalContactModel>();

            var sql = $@"
                SELECT
                  c.forename AS FirstName,
                  c.surname AS LastName,
                  c.mobile AS PhoneMobile,
                  c.fax AS PhoneFax,
                  c.fast_dial AS PhoneOffice,
                  c.email AS EmailBusiness,
                  a.address AS Address1,
                  a.county AS StateProvince,
                  a.postcode AS PostalCode,
                  a.town AS City

                FROM public.contacts c 
                INNER JOIN public.addresses a ON c.address_id = a.id
                WHERE c.company_id = {sqlId}
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            return queryResult.ToList();

        }

        public static async Task<List<iMetalContactModel>> GetCompanyContacts(string coid, string code)
        {
            var sqlQuery = new SqlQuery<iMetalContactModel>();

            var sql = $@"
                SELECT
                  c.forename AS FirstName,
                  c.surname AS LastName,
                  c.mobile AS PhoneMobile,
                  c.fax AS PhoneFax,
                  c.fast_dial AS PhoneOffice,
                  c.email AS EmailBusiness,
                  a.address AS Address1,
                  a.county AS StateProvince,
                  a.postcode AS PostalCode,
                  a.town AS City

                FROM public.companies cy
                INNER JOIN public.contacts c ON cy.id = c.company_id
                INNER JOIN public.addresses a ON c.address_id = a.id
                WHERE cy.code = '{code}'
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            return queryResult.ToList();

        }



        public static async Task<List<iMetalFreightTerm>> GetFreightTerms(string coid)
        {
            var sqlQuery = new SqlQuery<iMetalFreightTerm>();

            var sql = $@"
                SELECT
                    '{coid}' AS Coid,
                    ttc.code AS Code,
                    ttc.description AS Description
                FROM public.transport_type_codes ttc
                WHERE ttc.status = 'A' and ttc.type = 'S'
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            return queryResult.ToList();
        }

        public static async Task<List<iMetalPaymentTerm>> GetPaymentTerms(string coid)
        {
            var sqlQuery = new SqlQuery<iMetalPaymentTerm>();

            var sql = $@"
                SELECT
                    '{coid}' AS Coid,
                    t.code AS Code,
                    t.description AS Description
                FROM public.terms t
                WHERE t.status = 'A'
                ";

            var queryResult = await sqlQuery.ExecuteQueryAsync(coid, sql);

            return queryResult.ToList();

        }

        public static async Task<iMetalCurrencyCode> GetCompanyCurrencyCode(string coid, string code)
        {
            var sqlQuery = new SqlQuery<iMetalCurrencyCode>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                $@"
            SELECT
                cc.code AS Currency
            FROM companies c 
            INNER JOIN currency_codes cc ON c.currency_id = cc.id
            WHERE c.Code = '{code}' AND c.type_id = 1 AND c.Status = 'A'
            ");

            return result.FirstOrDefault();
        }

        public static async Task<iMetalCompanyCreditRuleResult> GetCompanyCreditRuleResult(string coid, string companyCode)
        {
            var sqlQuery = new SqlQuery<iMetalCompanyCreditRuleResult>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                $@"
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
    
            WHERE c.Code = '{companyCode}' AND c.type_id = 1 AND ccr.Status = 'A'");

            return result.FirstOrDefault();
        }

        public static async Task<List<iMetalInvoice>> GetInvoices(string coid, string companyCode)
        {
            var sqlQuery = new SqlQuery<iMetalInvoice>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                $@"
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
             ");

            return result.ToList();
        }

        public static async Task<iMetalCompanyCreditTotalResults> GetCompanyCreditTotalResults(string coid, string companyCode)
        {
            var sqlQuery = new SqlQuery<iMetalCompanyCreditTotalResults>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                    $@"
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
            WHERE c.code='{companyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD') AND (ssc.code<>'CMP' AND ssc.code<>'DEL')");

            return result.First();
        }

        public static async Task<iMetalTotalOpenCompletedAndLargestResult> GetTotalOpenCompletedAndLargest(string coid, string companyCode)
        {

            var sqlQuery = new SqlQuery<iMetalTotalOpenCompletedAndLargestResult>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                $@"
            SELECT 
                MAX(COALESCE(st.balance_value, 0)) AS LargestInvoice,
                SUM(CASE WHEN ssc.code <> 'CMP' AND ssc.code<>'DEL' THEN 1 ELSE 0 END) AS OpenInvoices,
                SUM(CASE WHEN ssc.code  = 'CMP' THEN 1 ELSE 0 END) AS CompletedInvoices
            FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
                INNER JOIN companies c ON sh.customer_id=c.id
                INNER JOIN sales_types ON sh.type_id=sales_types.id
                INNER JOIN sales_totals st ON si.sales_total_id=st.id
                INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
            WHERE c.code='{companyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD')");

            return result.First();
        }
    }
    
}
