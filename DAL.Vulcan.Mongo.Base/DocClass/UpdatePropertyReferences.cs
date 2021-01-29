using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public static class UpdatePropertyReferences
    {
        public static void Execute(Object anyObject)
        {/*
            var objectType = anyObject.GetType();
            var properties = objectType.GetProperties();

            foreach (var propertyInfo in properties)
            {

                if (propertyInfo.PropertyType.Name == "PropertyValueRef")
                {
                    var value = (PropertyValueRef)propertyInfo.GetValue(anyObject, null);
                    if (value == null) continue;

                    var propertyTypeValue = new RepositoryBase<PropertyValue>().Find(value.Id);
                    if (value.Code != propertyTypeValue.Code)
                    {
                        value.Code = propertyTypeValue.Code;
                    }

                    propertyInfo.SetValue(anyObject, value);
                }
                else
                {
                    var propertyValue = propertyInfo.GetValue(anyObject, null);
                    if (propertyValue != null)
                    {
                        var propertyValueType = propertyValue.GetType();
                        //var descendsFrom = propertyValueType.BaseType;

                        if (propertyValueType.Name.Contains("ReferenceObject"))
                        {

                            Console.WriteLine(propertyInfo.Name);
                        }
                    }
                    
                }
                */
            }
        }
    }

    public static class UpdateReferenceObjectValues
    {
        public static void Execute(Object anyObject)
        {
            var objectType = anyObject.GetType();
            var properties = objectType.GetProperties();

            foreach (var obj in properties)
            {
                var baseType = obj.PropertyType.BaseType;

                if (baseType.Name.Contains("ReferenceObject"))
                {

                    var genericType = baseType.GetGenericTypeDefinition();
                    if (genericType == typeof(ReferenceObject<>))
                    {
                        // Figure out what generic args were used to make this thing
                        var genArgs = baseType.GetType().GetGenericArguments();

                        // fetch the actual typed variant of Foo
                        var typedVariant = genericType.MakeGenericType(genArgs);


                        // alternatively, we can say what the type of T is...
                        var typeofT = baseType.GetType().GetGenericArguments().First();

                       
                    }
                }

            }
        }
    }


