using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Queries;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.Mongo.Core.Test.NunitTests.CrmQuoteTests
{
    [TestFixture]
    internal class Tests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void GetLastQuote()
        {
            var crmQuote = CrmQuote.Helper.Find(x => x.QuoteId == 17029).First();
            foreach (var crmQuoteItemRef in crmQuote.Items)
                Console.WriteLine($"{crmQuoteItemRef.Index} - {crmQuoteItemRef.ItemSummaryViewModel.Total}");
        }

        [Test]
        public void GetQuotesPipeline()
        {
            var helperQuotes = new HelperQuote();
            var crmUser = CrmUser.Helper.Find(x => x.User.LastName == "Gallegos").First();
            var beginDate = DateTime.Now.AddMonths(-1).Date;
            var endDate = DateTime.Now.AddDays(1).Date;
            var forTeam = true;
            var showExpired = false;

            var results = new QuotesPipelineQuery(crmUser.UserId, beginDate, endDate, forTeam, showExpired);

            // var options = new JsonSerializerOptions
            // {
            //     IgnoreNullValues = false,
            //     WriteIndented = true,
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //     DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            //     MaxDepth = 10,
            //     Converters = { new DynamicJsonConverter()}
            //   
            //
            // };
            //dynamic result = new ExpandoObject();
            // result.QuotePipeline = new QuotesPipelineQuery(crmUser.UserId, beginDate, endDate, forTeam, showExpired);

            // var json = JsonSerializer.Serialize(result, options);
            // Console.WriteLine(json);
        }
    }

    public class DynamicJsonConverter : JsonConverter<dynamic>
    {
        public override dynamic Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True) return true;

            if (reader.TokenType == JsonTokenType.False) return false;

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var l)) return l;

                return reader.GetDouble();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                if (reader.TryGetDateTime(out var datetime)) return datetime;

                return reader.GetString();
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var documentV = JsonDocument.ParseValue(ref reader);
                return ReadObject(documentV.RootElement);
            }

            // Use JsonElement as fallback.
            // Newtonsoft uses JArray or JObject.
            var document = JsonDocument.ParseValue(ref reader);
            return document.RootElement.Clone();
        }

        private object ReadObject(JsonElement jsonElement)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();
            foreach (var obj in jsonElement.EnumerateObject())
            {
                var k = obj.Name;
                var value = ReadValue(obj.Value);
                expandoObject[k] = value;
            }

            return expandoObject;
        }

        private object ReadValue(JsonElement jsonElement)
        {
            object result = null;
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:
                    result = ReadObject(jsonElement);
                    break;
                case JsonValueKind.Array:
                    result = ReadList(jsonElement);
                    break;
                case JsonValueKind.String:
                    //TODO: Missing Datetime&Bytes Convert
                    result = jsonElement.GetString();
                    break;
                case JsonValueKind.Number:
                    //TODO: more num type
                    result = 0;
                    if (jsonElement.TryGetInt64(out var l)) result = l;
                    break;
                case JsonValueKind.True:
                    result = true;
                    break;
                case JsonValueKind.False:
                    result = false;
                    break;
                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    result = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        private object ReadList(JsonElement jsonElement)
        {
            IList<object> list = new List<object>();
            foreach (var item in jsonElement.EnumerateArray()) list.Add(ReadValue(item));
            return list.Count == 0 ? null : list;
        }

        public override void Write(Utf8JsonWriter writer,
            object value,
            JsonSerializerOptions options)
        {
            // writer.WriteStringValue(value.ToString());
        }
    }
}