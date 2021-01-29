using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class CompanyRemoveDuplicateAddresses
    {
        public void Execute(Company company)
        {
            var addresses = company.Addresses.Distinct().ToList();
            if (company.Addresses.Count > addresses.Count)
            {
                company.Addresses = addresses;
            }

            var distinctHashCodes = addresses.Select(x => x.HashCode).Distinct().ToList();
            var addressesToRemove = new List<Guid>();

            foreach (var hashCode in distinctHashCodes)
            {
                var lastAddressWithThisHashCode = addresses.Last(x => x.HashCode == hashCode);
                var duplicatesFound = addresses
                    .Where(x => x.HashCode == lastAddressWithThisHashCode.HashCode &&
                                x.Id != lastAddressWithThisHashCode.Id).Select(x => x.Id).ToList();
                if (duplicatesFound.Any())
                {
                    addressesToRemove.AddRange(duplicatesFound);
                }
            }

            if (addressesToRemove.Any())
            {
                foreach (var id in addressesToRemove)
                {
                    var address = company.Addresses.FirstOrDefault(x => x.Id == id);
                    if (address != null) company.Addresses.Remove(address);
                }
            }

        }
    }
}