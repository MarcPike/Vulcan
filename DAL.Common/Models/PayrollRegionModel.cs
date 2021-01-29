using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class PayrollRegionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public PayrollRegionModel()
        {
            
        }

        public PayrollRegionModel(PayrollRegion p)
        {
            Id = p.Id.ToString();
            Name = p.Name;
        }
    }
}