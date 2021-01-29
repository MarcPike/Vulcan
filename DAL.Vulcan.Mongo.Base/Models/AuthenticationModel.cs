using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Base.Models
{
    public class AuthenticationModel
    {
        public string NetworkId { get; set; }
        public string Password { get; set; }

        public AuthenticationModel()
        {
        }

    }
}
