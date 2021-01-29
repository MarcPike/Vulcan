using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class OemType: BaseDocument
    {
        public string Name { get; set; }
    }
}
