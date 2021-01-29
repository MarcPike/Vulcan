using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BI.DAL.Mongo.Security;

namespace BI.WebApi.Controllers
{
    public class DeveloperToken
    {
        public static BiUserToken Token { get; set; }

    }
}
