using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Test;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.PhoneBook
{
    [TestFixture]
    public class PhoneBookTest
    {
        [Test]
        public void BasicTest()
        {
            var phoneBook = new Mongo.Test.PhoneBook();
            phoneBook.Items.Add(new PhoneBookItem()
            {
                Name = "Frankie",
                PhoneNumber = "777-777-7777"
            });
            phoneBook.SaveToDatabase();
        }

        [Test]
        public void FetchTest()
        {
            var phoneBookEntries = new RepositoryBase<Mongo.Test.PhoneBook>().AsQueryable().ToList();

            foreach (var phoneBook in phoneBookEntries)
            {
                foreach (var item in phoneBook.Items.ToList())
                {
                    ObjectDumper.Dump(item);
                }
            }
        }
    }
}
