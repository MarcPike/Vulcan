using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;

namespace DAL.Common.Test.Tests
{
    public enum Fruit
    {
        Apple,
        Banana,
        Cherry,
        Peach,
        Blueberry
    }

    public enum DessertType
    {
        Pie,
        Cake,
        Cobbler,
        Muffin
    }

    public class Dessert
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public Fruit Fruit { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public DessertType Type { get; set; }
    }
    [TestFixture]
    public class TestExternalObjectsList
    {

        [Test]
        public void SaveExternalDocumentsToTelge()
        {
            var queryHelper = new CommonMongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Office == "Telge");
            var telge = queryHelper.Find(filter).Single();

            var applePie = new Dessert()
            {
                Name = "Apple Pie",
                Fruit = Fruit.Apple,
                Type = DessertType.Pie
            };
            var peachCobbler = new Dessert()
            {
                Name = "Peach Cobbler",
                Fruit = Fruit.Peach,
                Type = DessertType.Cobbler
            };

            var blueberryMuffin = new Dessert()
            {
                Name = "Blueberry Muffin",
                Fruit = Fruit.Blueberry,
                Type = DessertType.Muffin
            };

            var desserts = new List<Dessert>()
            {
                applePie,
                peachCobbler,
                blueberryMuffin
            };

            var documentHelper = new ExternalDocumentHelper<Location, Dessert>();
            documentHelper.SaveDocumentsFor(telge,"Desserts", desserts);

            queryHelper.Upsert(telge);
        }

        [Test]
        public void GetExternalDocumentsFromTelge()
        {
            var queryHelper = new CommonMongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Office == "Telge");
            var telge = queryHelper.Find(filter).Single();

            var documentHelper = new ExternalDocumentHelper<Location, Dessert>();
            var desserts = documentHelper.GetDocumentsFor(telge, "Desserts");
            foreach (var dessert in desserts)
            {
                Console.WriteLine($"{dessert.Name}");
            }
        }

        [Test]
        public void RemoveExternalDocumentsFromTelge()
        {
            var queryHelper = new CommonMongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Office == "Telge");
            var telge = queryHelper.Find(filter).Single();

            telge.RemoveExternalDocumentList("Desserts");
            queryHelper.Upsert(telge);

        }

    }
}
