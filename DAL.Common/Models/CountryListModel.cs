using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class CountryListModel
    {
        public List<CountryValueModel> Countries { get; set; } = new List<CountryValueModel>();

        public CountryListModel()
        {
            
        }

        public static CountryListModel GetModel()
        {
            var model = new CountryListModel();
            foreach (var countryValue in CountryValue.Helper.GetAll().OrderBy(x=>x.CountryName))
            {
                model.Countries.Add(new CountryValueModel(countryValue));
            }

            return model;
        }

        public static CountryListModel SaveModel(CountryListModel model)
        {
            var currentCountries = CountryValue.Helper.GetAll();
            foreach (var country in model.Countries)
            {
                var thisCountryValue = currentCountries.FirstOrDefault(x => x.CountryName == country.CountryName);
                if (thisCountryValue == null)
                {
                    thisCountryValue = new CountryValue() 
                    {
                        CountryName = country.CountryName,
                    };
                }

                thisCountryValue.States = country.States.Select(x => new StateValue(x.StateName))
                    .OrderBy(x => x.StateName).ToList();
                CountryValue.Helper.Upsert(thisCountryValue);

            }

            return GetModel();
        }
    }
}