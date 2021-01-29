using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.ExternalLogin
{
    public class ExternalLogin : BaseDocument
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
