using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class CompanySaveShippingAddress
    {
        public void Execute(Company company, Address shipToAddress)
        {
            var addressFound = company.Addresses.FirstOrDefault(x => x.HashCode == shipToAddress.HashCode);

            if (addressFound == null)
            {
                shipToAddress.Type = AddressType.ShippingNew;
                company.Addresses.Add(shipToAddress);
            }
            else
            {
                if (shipToAddress.Type != addressFound.Type)
                {
                    shipToAddress.Type = addressFound.Type;
                }
            }

        }
    }
}