using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HelperPropertiesTests
{
    [TestFixture]
    public class HelperPropertiesTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void GetListOfMedicalInfoProps()
        {
            var propertyValues = PropertyValue.Helper.
                Find(x => x.Type == "MedicalInformationDocumentType").ToList();

            foreach (var propertyValue in propertyValues)
            {
                Console.WriteLine($"{propertyValue.Code} - {propertyValue.Description}");
            }
        }
        
        [Test]
        public void RemoveInvalidTrainingGroupClassification()
        {

            var propertyValues = PropertyValue.Helper
                .Find(x => (x.Type == "TrainingGroupClassification")  && !(x.Code == "HR" || x.Code == "HSE" || x.Code == "HR and HSE")).ToList();
            foreach (var propertyValue in propertyValues)
            {
                Console.WriteLine($"{propertyValue.Code} - {propertyValue.Description} Active: {propertyValue.Active}");
                PropertyValue.Helper.DeleteOne(propertyValue.Id);
            }



        }

        [Test]
        public void RemoveEmployeeDetailsDocumentType()
        {
            var property = PropertyType.Helper.Find(x => x.Type == "Employee DetailsDocumentType").Single();
            Assert.IsNotNull(property);
            var values = property.PropertyValues.ToList();
            foreach (var propertyValue in values)
            {
                PropertyValue.Helper.DeleteOne(propertyValue.Id);
            }

            PropertyType.Helper.DeleteOne(property.Id);
        }

        [Test]
        public void CostCentersModelSort()
        {
            // my code for all properties
            var helperProperties = new HelperProperties();
            // What the route is calling
            var model = helperProperties.GetAllProperties().First(x => x.Type == "CostCenter");
            // should be the sort you are seeing
            foreach (var propertyValue in model.PropertyValues)
            {
                Console.WriteLine($"Id: {propertyValue.Id} {propertyValue.Code} - {propertyValue.Description} Entity: {propertyValue.Entity.Name} Active: {propertyValue.Active}");
            }
        }

        [Test]
        public void KronosPayRuleSort()
        {
            // my code for all properties
            var helperProperties = new HelperProperties();
            // What the route is calling
            var model = helperProperties.GetAllProperties().First(x => x.Type == "KronosPayRuleType");
            // should be the sort you are seeing
            foreach (var propertyValue in model.PropertyValues)
            {
                Console.WriteLine($"Id: {propertyValue.Id} {propertyValue.Code} - {propertyValue.Description} Entity: {propertyValue.Entity.Name} Active: {propertyValue.Active}");
            }
        }


    }
}
