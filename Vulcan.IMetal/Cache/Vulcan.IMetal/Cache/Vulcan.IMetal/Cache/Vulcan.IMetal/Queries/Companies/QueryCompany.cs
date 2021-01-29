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

        public IQueryable<Company> BuildQuery(bool withAddresses)
        {
            IQueryable<Company> companies;
            if (withAddresses)
            {
                companies = Context.Company.
                    LoadWith(x => x.CompanyTypeCode).
                    LoadWith(x => x.CompanySubAddress).
                    LoadWith(x => x.SalesGroup);
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
    }
}
