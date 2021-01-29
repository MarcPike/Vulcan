using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class LocationModel
    {
        public string Id { get; set; }
        public string Coid { get; set; }
        public string Branch { get; set; } // eur usa
        public string Region { get; set; } // meap, Europe, Western
        public string Country { get; set; } // country UAE, Scotland, USA, Norway
        public string Office { get; set; } // Telge
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PhoneTollFree { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public MapLocation MapLocation { get; set; }
        public EntityRef Entity { get; set; }

        public string Locale { get; set; } = string.Empty;
        public CurrencyTypeRef DefaultCurrency { get; set; }
        public string KronosLaborLevel { get; set; }
        public List<PayrollRegionRef> PayrollRegions { get; set; } = new List<PayrollRegionRef>();

        public LocationTimeZoneRef TimeZone { get; set; } 

    }
}
