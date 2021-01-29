using DAL.Common.DocClass;
using System.Collections.Generic;
using Address = DAL.Common.DocClass.Address;
using MapLocation = DAL.Common.DocClass.MapLocation;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;

namespace DAL.HRS.Mongo.Models
{
    //public class LocationModel
    //{
    //    public string Id { get; set; }
    //    public string Branch { get; set; } // eur usa
    //    public string Region { get; set; } // meap, Europe, Western
    //    public string Country { get; set; } // country UAE, Scotland, USA, Norway
    //    public string Office { get; set; } // Telge
    //    public string Phone { get; set; }
    //    public string Fax { get; set; }
    //    public string PhoneTollFree { get; set; }
    //    public List<Address> Addresses { get; set; } = new List<Address>();

    //    public MapLocation MapLocation { get; set; }

    //    public List<PayrollRegionRef> PayrollRegions { get; set; } = new List<PayrollRegionRef>();

    //    public EntityRef Entity { get; set; }
    //    public string Coid { get; set; }

    //    public LocationModel()
    //    {
    //    }

    //    public LocationModel(Location l)
    //    {
    //        Id = l.Id.ToString();
    //        Branch = l.Branch;
    //        Region = l.Region;
    //        Country = l.Country;
    //        Office = l.Office;
    //        Phone = l.Phone;
    //        Fax = l.Fax;
    //        PhoneTollFree = l.PhoneTollFree;
    //        Addresses = l.Addresses;
    //        MapLocation = l.MapLocation;
    //        PayrollRegions = l.PayrollRegions;
    //        Entity = l.Entity;
    //        Coid = l.Coid;
    //    }
    //}
}
