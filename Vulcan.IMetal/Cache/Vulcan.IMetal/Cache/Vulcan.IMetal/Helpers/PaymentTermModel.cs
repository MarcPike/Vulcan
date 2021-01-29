namespace Vulcan.IMetal.Helpers
{
    public class PaymentTermModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DueDays { get; set; }
    }
}