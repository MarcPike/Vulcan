using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.WebApi2.Helpers
{
    public class LdapHelper : ILdapHelper
    {
        public string Host => "howcogroup.com";
        public int Port => 389;
        public string Domain => "howco";
    }
}
