using System.Collections.Generic;
using Vulcan.IMetal.Context.Company;

namespace Vulcan.IMetal.Queries.Companies
{
    public class iMetalCompany
    {
        public int SqlId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Address Address { get; set; }
        public int? StatusId { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}