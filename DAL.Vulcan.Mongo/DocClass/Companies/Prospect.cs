using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    public class Prospect: BaseDocument
    {
        //public string Code { get; set; }
        public static MongoRawQueryHelper<Prospect> Helper = new MongoRawQueryHelper<Prospect>();

        public string Name { get; set; }
        //public string ShortName { get; set; }
        public string Branch { get; set; }
        public LocationRef Location { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public List<ContactRef> Contacts { get; set; } = new List<ContactRef>();
        public CompanyRef Company { get; set; }
        public DateTime? AddedToSystem { get; set; }

        public bool IsActive => Company != null;

        public ProspectRef AsProspectRef()
        {
            return new ProspectRef(this);
        }
    }
}
