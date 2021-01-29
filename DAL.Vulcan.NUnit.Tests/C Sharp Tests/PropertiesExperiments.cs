using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.C_Sharp_Tests
{

    [TestFixture]
    public class PropertiesExperiments
    {
        public bool IsOn { get; set; } = true;

        private string _label;
        public string Label
        {
            get
            {
                if (IsOn)
                {
                    _label = "I AM TURNED ON";
                    
                }

                return _label;
            }
            set => _label = IsOn ? "I AM TURNED ON" : value;
        }

        [Test]
        public void Expiriments()
        {

            Assert.AreEqual(Label,"I AM TURNED ON");
            IsOn = false;
            Label = "turned off";
            Assert.AreEqual(Label, "turned off");

        }

        [Test]
        public void TestPerson()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var isidro = new Person("TestPerson", 4);
            isidro.SaveToDatabase();
            var marc = new Person("Marc",5);
            marc.SaveToDatabase();
        }

        [Test]
        public void FindPerson()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<Person>();
            var marc = rep.AsQueryable().FirstOrDefault(x => x.Label == "Marc");
            Console.WriteLine(ObjectDumper.Dump(marc));
        }

    }

    public class Person: BaseDocument
    {
        public string Label { get; set; }
        public int Girlfriends { get; set; }

        public Person()
        {
        }

        public Person(string label, int girls)
        {
            Label = label;
            Girlfriends = girls;
        }

    }
}
