using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class PropertyValueRef 
    {
        public string Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public EntityRef Entity { get; set; }

    }
}