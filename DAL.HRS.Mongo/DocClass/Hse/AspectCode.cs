using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class AspectCode
    {
        public string Description { get; set; }
        public PropertyValueRef Type { get; set; }
        public PropertyValueRef OpportunityType { get; set; }
        public LocationRef Location { get; set; }
        public int Index { get; set; }
    }
}