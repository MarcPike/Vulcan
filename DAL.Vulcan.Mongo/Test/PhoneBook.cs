using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Test
{
    public class PhoneBook: BaseDocument
    {
        public List<PhoneBookItem> Items { get; set; } = new List<PhoneBookItem>();
    }

    public class PhoneBookItem
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
