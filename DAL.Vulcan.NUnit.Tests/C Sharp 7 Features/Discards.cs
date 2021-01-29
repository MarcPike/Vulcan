using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static System.ValueTuple;

namespace DAL.Vulcan.NUnit.Tests.C_Sharp_7_Features
{
    // I do not understand how these damn Discards work

    [TestFixture]
    public class Discards
    {

        public Discards((int Id, string Name, int Age) personTuple)
        {
        }

        public IQueryable<(int Id, string Name, int Age)> GetPersonTuples()
        {
            var result = new List<(int Id, string Name, int Age)>
            {
                (1, "Ken", 48),
                (1, "Marc", 50),
                (1, "TestPerson", 34),
                (1, "Anu", 45)
            };

            return result.AsQueryable();
        }

        //private (int, string, int) QueryPersons(int id, int name, int age)
        //{
        //    var persons = GetPersonTuples();
        //    if (id )


        //}

        [Test]
        public void QueryUsingDiscards()
        {
            
        }
    }
}
