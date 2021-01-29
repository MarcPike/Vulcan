using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;

namespace DAL.Common.Models
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

        public LocationModel()
        {
        }

        public LocationModel(Location l)
        {
            Id = l.Id.ToString();
            Coid = l.Coid;
            Branch = l.Branch;
            Region = l.Region;
            Country = l.Country;
            Office = l.Office;
            Phone = l.Phone;
            Fax = l.Fax;
            PhoneTollFree = l.PhoneTollFree;
            Addresses = l.Addresses;
            MapLocation = l.MapLocation;
            Entity = l.Entity;
            DefaultCurrency = l.DefaultCurrency;
            KronosLaborLevel = l.KronosLaborLevel;
            PayrollRegions = l.PayrollRegions;
            TimeZone = l.TimeZone?.AsLocationTimeZone().AsLocationTimeZoneRef();
            Locale = l.Locale;
        }
    }
}
