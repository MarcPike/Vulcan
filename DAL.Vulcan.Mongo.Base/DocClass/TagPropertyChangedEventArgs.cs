using System.ComponentModel;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class TagPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public TagPropertyChangedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}