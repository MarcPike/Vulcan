using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.CRM
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
