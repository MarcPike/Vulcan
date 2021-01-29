using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class CountryValueModel
    {
        public string Id { get; set; }
        public string CountryName { get; set; }
        public List<StateValueModel> States { get; set; } = new List<StateValueModel>();

        public CountryValueModel()
        {
            
        }

        public CountryValueModel(CountryValue country)
        {
            Id = country.Id.ToString();
            CountryName = country.CountryName;
            foreach (var stateValue in country.States.OrderBy(x=>x.StateName))
            {
                States.Add(new StateValueModel(stateValue));
            }
        }

    }
}
