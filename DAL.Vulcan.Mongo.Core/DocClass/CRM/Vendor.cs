using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class Vendor: BaseDocument
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public LocationRef Location { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}
