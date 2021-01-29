using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using System.Linq;
using System.Reflection;
using AutoMapper;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Base.DocClass
{

    [BsonIgnoreExtraElements]
    public class ReferenceObject<TBaseDocument>
        where TBaseDocument : BaseDocument, IEqualityComparer<TBaseDocument>
    {
        private static MongoRawQueryHelper<TBaseDocument> _helper = new MongoRawQueryHelper<TBaseDocument>();
        public string Id { get; set; } = string.Empty;

        public bool GetIsDeleted()
        {
            var document = ToBaseDocument();

            if (document == null)
            {
                return true;
            }

            return false;
        }

        public TBaseDocument ToBaseDocument()
        {
            return _helper.FindById(Id);
        }

        public ReferenceObject()
        {
            
        }

        public ReferenceObject(TBaseDocument document)
        {
            if (document == null)
            {
                return;
            }
            
            Id = document.Id.ToString();
            GetPropertiesFromBaseObject(document);
        }

        public System.Type GetBaseDocumentType()
        {
            return typeof(TBaseDocument);
        }

        public void RefreshPropertyValues()
        {
            var baseDocument = ToBaseDocument();
            if (baseDocument == null)
            {
                return;
            }
            GetPropertiesFromBaseObject(baseDocument);
        }

        public void GetPropertiesFromBaseObject(TBaseDocument baseDocument)
        {
            //try
            //{
                //var baseDocument = ToBaseDocument();
                PropertyInfo[] sourceAllProperties = baseDocument.GetType().GetProperties();

                foreach (PropertyInfo sourceProperty in sourceAllProperties)
                {
                    if (sourceProperty.Name == "Id") continue;

                if (!sourceProperty.CanWrite)
                {
                    continue;
                }

                PropertyInfo selfProperty = this.GetType().GetProperty(sourceProperty.Name);
                    if (selfProperty != null)
                    {
                        var sourceValue = sourceProperty.GetValue(baseDocument, null);
                        selfProperty.SetValue(this, sourceValue, null);
                    }
                }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
        }

        public void GetPropertiesFromObject(object someObject)
        {
            var properties = GetProperties(someObject);

            foreach (var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(someObject, null);
                if (name == "Id") continue;

                PropertyInfo selfProperty = this.GetType().GetProperty(name);
                if (selfProperty != null)
                {
                    selfProperty.SetValue(this, value, null);
                }

            }
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

    }
}