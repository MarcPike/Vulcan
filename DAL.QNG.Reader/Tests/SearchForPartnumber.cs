using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.QNG.Reader.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.QNG.Reader.Tests
{
    [TestFixture]
    public class SearchForPartnumber
    {
        private ODSReader _context = new ODSReader();



        [SetUp]
        public void SetUp()
        {
            //_context = new ODSReader(@"&quot;data source=S-US-DW01;initial catalog=ODS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
        }

        [Test]
        public void TestHalliburton()
        {
            var coid = "INC";
            var companyStartsWith = "Hal";
            var companies = GetCompanies(coid, companyStartsWith);
            var productCodes = GetProductsWithPartNumbers(companies);
            foreach (var product in productCodes)
            {
                Console.WriteLine($"[{product.CompanyCode}-{product.CompanyName}]  {product.CustomerPartNumber} - ({product.ProductId}) {product.ProductCode}");
            }
        }


        [Test]
        public List<Companies> GetCompanies(string coid, string companyStartsWith)
        {
            var companies = _context.Companies.Where(x => x.name.Contains(companyStartsWith) && x.COID == coid).ToList();
            //foreach (var company in companies)
            //{
            //    Console.WriteLine($"{company.id} {company.name}");
            //}

            return companies;
        }

        [Test]
        public List<PartNumbersForCustomer> GetProductsWithPartNumbers(List<Companies> companies)
        {
            var products = new List<PartNumbersForCustomer>();
            foreach (var company in companies)
            {
                var partNumbers = _context.PartNumberSpecifications.AsNoTracking().Where(x => x.customer_id == company.id).ToList();
                foreach (var partNumberSpecification in partNumbers.Where(x=>x.product_id != null))
                {
                    if (products.Any(x=>x.CustomerPartNumber == partNumberSpecification.number && x.ProductId == partNumberSpecification.product_id )) continue;


                    products.Add(new PartNumbersForCustomer()
                    {
                        CompanyCode = company.code,
                        CompanyName = company.name,
                        CustomerPartNumber = partNumberSpecification.number,
                        ProductId = partNumberSpecification.product_id ?? 0,
                        OD = partNumberSpecification.outside_diameter ?? 0,
                        ID = partNumberSpecification.inside_diameter ?? 0,
                    });
                }
            }

            foreach (var product in products)
            {
                product.ProductCode = _context.Products.FirstOrDefault(x => x.id == product.ProductId)?.code ?? string.Empty;
            }

            return products;
        }

    }
}
