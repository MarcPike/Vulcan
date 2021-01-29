using System;
using System.Collections.Generic;
using System.Linq;
using BI.DAL.Mongo.BiUserObjects;
using DAL.Common.DocClass;

namespace BI.DAL.Mongo.Models
{
    public class BiUserModel
    {
        public string Id { get; set; }
        public bool SystemAdmin { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Person Person { get; set; }
        public LocationRef Location {get;set;}
        public List<BiQueryBaseModel> Queries { get; set; } = new List<BiQueryBaseModel>();

        public BiUserModel()
        {
        }

        public BiUserModel(BiUser user)
        {
            Id = user.Id.ToString();
            UserId = user.UserId;
            SystemAdmin = user.SystemAdmin;
            Person = user.Person;
            Location = user.Location;
            Queries = user.Queries.Select(x => new BiQueryBaseModel(x)).ToList();
        }

    }
}
