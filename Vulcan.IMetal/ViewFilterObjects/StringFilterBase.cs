using System;
using System.Collections.Generic;
using Devart.Data.Linq;
using Newtonsoft.Json;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public class StringFilterBase: FilterBase
    {
        public StringFilterType StringFilterType { get; set; } = StringFilterType.Contains;
        public string StringFilterName => StringFilterType.ToString();

        [JsonIgnore]
        public bool IsActive => (Value != string.Empty) || (Values.Count > 0);

        public string Value { get; set; } = string.Empty;
        public List<string> Values { get; set; } = new List<string>();

        public StringFilterBase()
        {
            Kind = "StringFilter";
        }

    public string BuildInStringList(string value)
        {
            var result = string.Empty;
            var comma = string.Empty;
            foreach (var stringVal in Value)
            {
                result += comma + $"'{stringVal}'";
                comma = ",";
            }
            return result;
        }

        public void AddInList(string value)
        {
            Values.Add(value);
            StringFilterType = StringFilterType.InList;
        }

        public void Equals(string value)
        {
            Value = value;
            StringFilterType = StringFilterType.EqualTo;
        }

        public void Contains(string value)
        {
            Value = value;
            StringFilterType = StringFilterType.Contains;
        }

        public void StartsWith(string value)
        {
            Value = value;
            StringFilterType = StringFilterType.StartsWith;
        }

    }
}