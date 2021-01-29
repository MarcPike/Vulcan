using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.ExternalLogin;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ExternalLoginModel
    {
        public string AdminUserId { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ExternalLoginModel() { }

        public ExternalLoginModel(ExternalLogin login, string adminUserId)
        {
            Id = login.Id.ToString();
            UserName = login.UserName;
            Password = login.Password;
            AdminUserId = adminUserId;
        }
    }
}
