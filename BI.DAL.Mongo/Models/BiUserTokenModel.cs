using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.DAL.Mongo.Security;

namespace BI.DAL.Mongo.Models
{
    public class BiUserTokenModel
    {
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public DateTime Expires { get; set; }
        public string FullName { get; set; }

        public string MiddleName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public BiUserTokenModel()
        {
            
        }

        public BiUserTokenModel(BiUserToken t)
        {
            UserId = t.User.Id;
            TokenId = t.TokenId;
            Expires = t.Expires;
            LastName = t.User.LastName;
            FirstName = t.User.FirstName;
            MiddleName = t.User.MiddleName;
            FullName = t.User.GetFullName();
        }

    }
}
