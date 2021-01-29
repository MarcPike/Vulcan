
using DAL.iMetal.Core.Models;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class CompanyListModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ItemMarginModel Margin { get; set; }
        //public List<OrderListModel> Orders { get; set; } = new List<OrderListModel>();

        public CompanyListModel(string code, string shortName, string name, ItemMarginModel margin)
        {
            Code = code;
            ShortName = shortName;
            Name = Name;
            Margin = margin;
        }
    }
}