using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Properties
{
    [TestFixture]
    public class CreateEdgenPropertyForEveryHowcoProperty
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            var queryHelperHowco = new MongoRawQueryHelper<PropertyValue>();
            var queryHelperEdgen = new MongoRawQueryHelper<PropertyValue>();
            var filter = queryHelperHowco.FilterBuilder.Where(x => x.Entity.Name == "Howco");

            var howcoProperties = queryHelperHowco.Find(filter).ToList();
            foreach (var howcoProperty in howcoProperties)
            {
                var filterEdgen = queryHelperEdgen.FilterBuilder.Where(x =>
                    x.Entity.Name == "Edgen Murray" && x.Code == howcoProperty.Code);
                if (!queryHelperEdgen.Find(filterEdgen).Any())
                {
                    queryHelperEdgen.Upsert(new PropertyValue()
                    {
                        Active = howcoProperty.Active,
                        Code = howcoProperty.Code,
                        Type = howcoProperty.Type,
                        Description = howcoProperty.Description,
                        Entity = Entity.GetRefByName("Edgen Murray"),
                    });
                }

            }

        }
    }
}
