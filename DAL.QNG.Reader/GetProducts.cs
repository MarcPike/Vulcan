using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.QNG.Reader.Tests;

namespace DAL.QNG.Reader
{
    public static class GetProducts
    {
        public static List<PartNumbersForCustomer> GetProductsFor(string coid, string companyNameStartsWith)
        {
            var searcher = new SearchForPartnumber();
            var companies = searcher.GetCompanies(coid, companyNameStartsWith);
            var productCodes = searcher.GetProductsWithPartNumbers(companies);
            return productCodes;
        }
    }
}
