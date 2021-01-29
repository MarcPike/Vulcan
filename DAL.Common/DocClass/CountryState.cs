using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.DocClass
{
    public class CountryState : BaseDocument, ICommonDatabaseObject
    {
        public string Country { get; set; }
        public string StateProvince { get; set; }
        public static MongoRawQueryHelper<CountryState> Helper = new MongoRawQueryHelper<CountryState>();

        private static void AddNew(string country, string stateProvince)
        {
            var dupFilter = Helper.FilterBuilder.Where(x => x.Country == country && x.StateProvince == stateProvince);
            var dup = Helper.Find(dupFilter).FirstOrDefault();
            if (dup != null) return;

            Helper.Upsert(new CountryState()
            {
                Country = country,
                StateProvince = stateProvince
            });
        }

        public static void Initialize()
        {
            CountryState.AddNew("Canada", "Northwest Territories");
            CountryState.AddNew("Canada", "British Columbia");
            CountryState.AddNew("Canada", "Ontario");
            CountryState.AddNew("Canada", "Quebec");
            CountryState.AddNew("Canada", "Prince Edward Island");
            CountryState.AddNew("Canada", "Alberta");
            CountryState.AddNew("Canada", "Nunavut");
            CountryState.AddNew("Canada", "Manitoba");
            CountryState.AddNew("Canada", "New Brunswick");
            CountryState.AddNew("Canada", "Saskatchewan");
            CountryState.AddNew("Canada", "Newfoundland & Labrador");
            CountryState.AddNew("Canada", "Yukon Territory");
            CountryState.AddNew("Canada", "Nova Scotia");
            CountryState.AddNew("United States", "Arizona");
            CountryState.AddNew("United States", "Arkansas");
            CountryState.AddNew("United States", "California");
            CountryState.AddNew("United States", "Colorado");
            CountryState.AddNew("United States", "New York");
            CountryState.AddNew("United States", "Connecticut");
            CountryState.AddNew("United States", "North Carolina");
            CountryState.AddNew("United States", "Deleware");
            CountryState.AddNew("United States", "Florida");
            CountryState.AddNew("United States", "North Dakota");
            CountryState.AddNew("United States", "Georgia");
            CountryState.AddNew("United States", "Hawaii");
            CountryState.AddNew("United States", "Idaho");
            CountryState.AddNew("United States", "Alabama");
            CountryState.AddNew("United States", "Ohio");
            CountryState.AddNew("United States", "Illinois");
            CountryState.AddNew("United States", "Oklahoma");
            CountryState.AddNew("United States", "Indiana");
            CountryState.AddNew("United States", "Iowa");
            CountryState.AddNew("United States", "Oregon");
            CountryState.AddNew("United States", "Kansas");
            CountryState.AddNew("United States", "Kentucky");
            CountryState.AddNew("United States", "Louisiana");
            CountryState.AddNew("United States", "Maine");
            CountryState.AddNew("United States", "Pennsylvania");
            CountryState.AddNew("United States", "Maryland");
            CountryState.AddNew("United States", "Rhode Island");
            CountryState.AddNew("United States", "Alaska");
            CountryState.AddNew("United States", "South Carolina");
            CountryState.AddNew("United States", "South Dakota");
            CountryState.AddNew("United States", "Tennessee");
            CountryState.AddNew("United States", "Massachusetts");
            CountryState.AddNew("United States", "Michigan");
            CountryState.AddNew("United States", "Texas");
            CountryState.AddNew("United States", "Minnesota");
            CountryState.AddNew("United States", "Mississippi");
            CountryState.AddNew("United States", "Missouri");
            CountryState.AddNew("United States", "Utah");
            CountryState.AddNew("United States", "Vermont");
            CountryState.AddNew("United States", "Montana");
            CountryState.AddNew("United States", "Nebraska");
            CountryState.AddNew("United States", "Virginia");
            CountryState.AddNew("United States", "Washington");
            CountryState.AddNew("United States", "Nevada");
            CountryState.AddNew("United States", "New Hampshire");
            CountryState.AddNew("United States", "West Virginia");
            CountryState.AddNew("United States", "New Jersey");
            CountryState.AddNew("United States", "Wisconsin");
            CountryState.AddNew("United States", "Wyoming");
            CountryState.AddNew("United States", "New Mexico");
            CountryState.AddNew("Unspecified", "Unspecified");
            CountryState.AddNew("England", "Derbyshire");
            CountryState.AddNew("England", "South Yorkshire");
            CountryState.AddNew("England", "South Yorks");
            CountryState.AddNew("England", "Sheffield");
            CountryState.AddNew("England", "Huddersfield");
            CountryState.AddNew("England", "West Yorkshire");
            CountryState.AddNew("England", "0");
            CountryState.AddNew("England", "England");
            CountryState.AddNew("England", "Doncaster");
            CountryState.AddNew("Scotland", "Lanarkshire");
            CountryState.AddNew("Scotland", "Ayrshire");
            CountryState.AddNew("Scotland", "Stirlingshire");
            CountryState.AddNew("Scotland", "Renfrewshire");
            CountryState.AddNew("Scotland", "Cumbernauld");
            CountryState.AddNew("Scotland", "Glasgow");
            CountryState.AddNew("Scotland", "Glasgow City");
            CountryState.AddNew("Scotland", "By Ayrshire");
            CountryState.AddNew("Scotland", "South Lanarkshire");
            CountryState.AddNew("Scotland", "Paisley");
            CountryState.AddNew("Scotland", "Ayshire");
            CountryState.AddNew("Scotland", "Aberdeen");
            CountryState.AddNew("Scotland", "Aberdeenshire");
            CountryState.AddNew("Scotland", "South Ayrshire");
            CountryState.AddNew("Scotland", "Irvine");
            CountryState.AddNew("United Kingdom", "South ");
            CountryState.AddNew("England", "Nottinghamshire");
            CountryState.AddNew("Scotland", "North Lanarkshire");
            CountryState.AddNew("Scotland", "East Kilbride");
            CountryState.AddNew("Singapore", "SINGAPORE");
            CountryState.AddNew("United Arab Emirates", "Ajman");
            CountryState.AddNew("United Arab Emirates", "Dubai");
            CountryState.AddNew("Scotland", "Unknown");
            CountryState.AddNew("Norway", "Norway");
            CountryState.AddNew("China", "Minhang District");
            CountryState.AddNew("China", "Songjiang District");
        }
    }
}
