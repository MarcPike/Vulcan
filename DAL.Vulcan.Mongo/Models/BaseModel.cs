using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class BaseModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public BaseModel()
        {
        }

        public BaseModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }
    }
}
