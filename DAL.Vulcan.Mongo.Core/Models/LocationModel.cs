using System;
using System.Collections.Generic;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Currency;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class LocationModel
    {
        public string Id { get; set; }

        public string Coid { get; set; }
        public string Branch { get; set; } // eur usa
        public string Region { get; set; } // meap, Europe, Western
        public string Country { get; set; } // country UAE, Scotland, USA, Norway
        public string Office { get; set; } // Telge
        public CurrencyType DefaultCurrency { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PhoneTollFree { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();

        public MapLocation MapLocation { get; set; }


        public LocationModel()
        {
            
        }

        public LocationModel(Location location)
        {
            Id = location.Id.ToString();
            Coid = location.GetCoid();
            Branch = location.Branch;
            Region = location.Region;
            Country = location.Country;
            Office = location.Office;
            DefaultCurrency = location.DefaultCurrency;
            Phone = location.Phone;
            Fax = location.Fax;
            PhoneTollFree = location.PhoneTollFree;
            Addresses = location.Addresses;
            MapLocation = location.MapLocation;

        }
    }
}
