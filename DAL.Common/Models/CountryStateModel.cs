using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class CountryStateModel
    {
        public string Country { get; set; }
        public List<string> StateProvinces { get; set; }

        public static List<CountryStateModel> GetCountryStateModel()
        {
            var result = new List<CountryStateModel>();
            var countryStateList = CountryState.Helper.GetAll();
            var countries = countryStateList.Select(x => x.Country).Distinct();
            foreach (var country in countries)
            {
                var model = new CountryStateModel() 
                {
                    Country = country,
                    StateProvinces = countryStateList
                        .Where(x=>x.Country == country)
                        .Select(x=>x.StateProvince)
                        .OrderBy(x=>x).ToList()
                };
                result.Add(model);
            }

            return result;

        }

    }
}
