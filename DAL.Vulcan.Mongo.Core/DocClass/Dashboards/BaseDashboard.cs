using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DateValues;
using DAL.Vulcan.Mongo.Core.DocClass.Currency;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Dashboards
{
    public class BaseDashboard: BaseDocument
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        public List<CurrencyType> AvailableCurrencies = CurrencyType.GetDefaults();
        public List<CoidCountry> AvailableCoidCountries = CoidCountry.GetDefaults();
        public List<DateValueItem> AvailableDateValues = DateValueItem.GetDateValues();
        public List<RollupBy> AvailableRollups { get; set; } = RollupBy.GetDefaults();

        public DateValueItem DateRangeChosen { get; set; } 
        public CurrencyType CurrencyChosen { get; set; } 
        public List<CoidCountry> CoidCountriesChosen { get; set; } = new List<CoidCountry>();
        public RollupBy ChosenRollup { get; set; } 

    }

    public class RollupBy
    {
        public string Value { get; set; }

        public static List<RollupBy> GetDefaults()
        {
            return new List<RollupBy>()
            {
                new RollupBy() { Value = "Coid"},
                new RollupBy() { Value = "Country"},
                new RollupBy() { Value = "Region"},
                new RollupBy() { Value = "SuperRegion"}
            };   
        }
    }
}

