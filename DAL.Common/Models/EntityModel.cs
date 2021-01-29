using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class EntityModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<LocationRef> Locations { get; set; }

        public EntityModel() { }

        public EntityModel(Entity e)
        {
            Id = e.Id.ToString();
            Name = e.Name;
            Locations = e.Locations;
        }
    }
}
