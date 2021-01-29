using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveUp.Net.WhoIs;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Rename_Field_Tests
{
    [TestFixture]
    public class RenameUsingBuilder
    {
        public class Truck : BaseDocument
        {
            public int Year { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Headlights { get; set; }
            public string Motor { get; set; }
        }

        public class Car 
        {
            public int Year { get; set; }
            public string Mke { get; set; }
            public string Model { get; set; }
        }

        public class CarLot : BaseDocument
        {
            public string Name { get; set; } = "Parkway Chevrolet";
            public List<Car> CarsOnLot { get; set; } = new List<Car>();
        }

        [SetUp]
        public void SetUp()
        {
            var context = new VulcanContext();
            context.Database.DropCollection("Car");
            context.Database.DropCollection("CarLot");

            var carLot = new CarLot();

            var camaro = new Car()
            {
                Year = 2016,
                Mke = "Chevrolet",
                Model = "Camaro"
            };

            carLot.CarsOnLot.Add(camaro);

            var challenger = new Car()
            {
                Year = 2016,
                Mke = "Dodge",
                Model = "Challenger"
            };

            carLot.CarsOnLot.Add(challenger);

            var rep = new RepositoryBase<CarLot>();
            rep.Upsert(carLot);


        }

        [Test]
        public void TruckTest()
        {
            var truck = new Truck()
            {
                Headlights = "Hallogen",
                Make = "Dodge",
                Model = "Ram",
                Motor = "Hemi",
                Year = 2015
            };

            new RepositoryBase<Truck>().Upsert(truck);
        }
        [Test]
        public void TruckTest2()
        {
            var rep = new RepositoryBase<Truck>();
            var truck = rep.AsQueryable().SingleOrDefault(x => x.Make == "Dodge");
            truck.SetTagValue("QuarterMile",13.5);
            rep.Upsert(truck);

        }

        [Test]
        /// https://docs.mongodb.com/manual/reference/command/eval/#dbcmd.eval
        public void CallExistingFunctionJavaScript()
        {
            var context = new VulcanContext();
            var num1 = 10;
            var num2 = 19;
            var command = new JsonCommand<BsonDocument>($"{{ eval: \"myAddFunction({num1},{num2})\" }}");
            var result = context.Database.RunCommand(command)["retval"].ToInt32();
            
            Console.WriteLine(result);
        }

        [Test]
        public void CallNativeJavaScript()
        {
            //var context = new VulcanContext();
            //var commandDoc = new FindCommandOperation<>(); @"_db.getCollection('Notification').find({}))";
            //var collection = new RepositoryBase<Notification>().Collection;
            //var result = collection.Find(command);
            //Console.WriteLine(result);

        }

        [Test]
        // This pattern works for everything except nested documents/arrays
        public void Test()
        {
            var context = new VulcanContext();
            var collection = context.Database.GetCollection<BsonDocument>("CarLot");
            var filter = Builders<BsonDocument>.Filter.Empty;
            var rename = Builders<BsonDocument>.Update.Rename("CarsOnLot.$.Mke", "CarsOnLot.$.Make");

            Console.WriteLine(rename.Render(new AbstractClassSerializer<BsonDocument>(), new BsonSerializerRegistry()));

            try
            {
                collection.UpdateMany(filter, rename, new UpdateOptions() {BypassDocumentValidation = true,IsUpsert = false});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [TearDown]
        public void TearDown()
        {
            var context = new VulcanContext();
            context.Database.DropCollection("Car");
        }
    }
}
