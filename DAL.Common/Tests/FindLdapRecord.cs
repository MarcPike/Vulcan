using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class FindLdapRecord
    {
        [Test]
        public void Execute()
        {
            var dups = LdapUser.Helper.Find(x => x.LastName == "Vongsamphanh").ToList();
            foreach (var ldapUser in dups)
            {
                Console.WriteLine($"{ldapUser.FirstName} {ldapUser.LastName} - {ldapUser.NetworkId}");
            }
        }
    }
}
