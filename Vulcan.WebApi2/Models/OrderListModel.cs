using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.IMetal.Models;

namespace Vulcan.WebApi2.Models
{
    public class OrderListModel
    {
        public string Coid { get; set; }
        public int Number { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string SalesPerson { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public ItemMarginModel Margin { get; set; }

        public OrderListModel(string coid, int number, string companyCode, string companyName, string companyShortName, string salesPerson, 
            DateTime? dueDate, DateTime? saleDate, ItemMarginModel margin)
        {
            Coid = coid;
            Number = number;
            CompanyCode = companyCode;
            CompanyName = companyName;
            CompanyShortName = companyShortName;
            SalesPerson = salesPerson;
            DueDate = dueDate;
            SaleDate = saleDate;
            Margin = margin;
        }

    }
}
