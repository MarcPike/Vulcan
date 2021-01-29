using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.SynchProperties
{
    [TestFixture]
    class SynchronizePropertiesAllEntities
    {
        private HelperProperties _helperProperties = new HelperProperties();
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            foreach (var propertyType in PropertyType.Helper.GetAll())
            {
                var howcoPropertyValues = PropertyValue.Helper
                    .Find(x => x.Type == propertyType.Type && x.Entity.Name == "Howco").ToList();
                var edgenPropertyValues = PropertyValue.Helper
                    .Find(x => x.Type == propertyType.Type && x.Entity.Name == "Edgen Murray").ToList();

                // Add to Edgen
                foreach (var value in howcoPropertyValues)
                {
                    bool update = false;
                    var otherEntityValue = edgenPropertyValues.FirstOrDefault(x => x.Code == value.Code && x.Type == value.Type);
                    if (otherEntityValue == null)
                    {
                        otherEntityValue = new PropertyValue()
                        {
                            Code = value.Code,
                            Type = value.Type,
                            Description = value.Description,
                            Entity = Entity.GetRefByName("Edgen Murray"),
                            Active = value.Active,
                        };
                        PropertyValue.Helper.Upsert(otherEntityValue);
                    } else if (otherEntityValue.Description != value.Description)
                    {
                        otherEntityValue.Description = value.Description;
                        PropertyValue.Helper.Upsert(otherEntityValue);
                    }
                }

                foreach (var value in edgenPropertyValues)
                {
                    bool update = false;
                    var otherEntityValue = howcoPropertyValues.FirstOrDefault(x => x.Code == value.Code && x.Type == value.Type);
                    if (otherEntityValue == null)
                    {
                        otherEntityValue = new PropertyValue()
                        {
                            Code = value.Code,
                            Type = value.Type,
                            Description = value.Description,
                            Entity = Entity.GetRefByName("Howco"),
                            Active = value.Active,
                        };
                        PropertyValue.Helper.Upsert(otherEntityValue);
                    }
                    else if (otherEntityValue.Description != value.Description)
                    {
                        otherEntityValue.Description = value.Description;
                        PropertyValue.Helper.Upsert(otherEntityValue);
                    }
                }

            }
        }

        [Test]
        public void CheckThisProperty()
        {
            var props = PropertyValue.Helper.Find(x => x.Type == "AwsEligible" ).ToList();
            foreach (var propertyValue in props)
            {
                Console.WriteLine($"{propertyValue.Code} - {propertyValue.Entity.Name}");
            }
        }

        //[Test]
        //public void CheckThisPropertyModel()
        //{
        //    var helperProperties = new HelperProperties();

        //    var properties = helperProperties.GetProperties("5dd40ee0095f595a7460a772");
        //    foreach (var propertyModel in properties)
        //    {
        //        Console.WriteLine($"{propertyModel.Code} - {propertyModel.Entity.Name}");
        //    }
        //}

    }
}
