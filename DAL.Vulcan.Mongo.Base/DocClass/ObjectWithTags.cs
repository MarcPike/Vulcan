using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class ObjectWithTags
    {
        [BsonElement("Tags")]
        public Dictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();

        public delegate void OnPropertyChangedHandler(object sender, TagPropertyChangedEventArgs args);
        public event BaseDocument.OnPropertyChangedHandler OnPropertyChanged;
        private void RaiseOnPropertyChanged(string key, object oldValue, object newValue)
        {
            OnPropertyChanged?.Invoke(key, new TagPropertyChangedEventArgs(key, oldValue, newValue));
        }


        public void SetTagValue(string key, object value)
        {
            if (Tags.ContainsKey(key))
            {
                Tags[key] = value;
                RaiseOnPropertyChanged(key, Tags[key], value);
            }
            else
            {
                Tags.Add(key, value);
                RaiseOnPropertyChanged(key, null, value);
            }

        }


        public object GetTagValue(string key, object defaultValue)
        {
            var result = Tags.SingleOrDefault(x => x.Key.Equals(key)).Value;

            if (result != null) return result;
            result = defaultValue;
            SetTagValue(key, defaultValue);

            return result;
        }
    }
}