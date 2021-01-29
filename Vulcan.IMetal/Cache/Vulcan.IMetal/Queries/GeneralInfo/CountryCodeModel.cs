using System.Collections.Generic;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Queries.GeneralInfo
{
    public class CountryCodeModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }
        public static List<CountryCodeModel> GetForCoid(string coid)
        {
            var result = new List<CountryCodeModel>();
            var context = ContextFactory.GetGeneralInfoContextForCoid(coid);
            foreach (var countryCode in context.CountryCode)
            {
                result.Add(new CountryCodeModel()
                {
                    Id = countryCode.Id,
                    Status = countryCode.Status,
                    Code = countryCode.Code,
                    Description = countryCode.Description,
                    Country = countryCode.Description
                });
            }

            return result;
        }
    }
}