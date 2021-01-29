using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class ChangeHistory
    {
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public HrsUserRef HrsUser { get; set; }
        public List<ValueChange> ValueChanges { get; set; } = new List<ValueChange>();
        public List<ListChange> ListChanges { get; set; } = new List<ListChange>();

        public bool ModificationsMade
        {
            get { return AnyChanges(); }
        }

        public bool AnyChanges()
        {
            return ValueChanges.Any() || ListChanges.Any();
        }
        public class ValueChange
        {
            public string FieldName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }

            public ValueChange(string fieldName, string oldValue, string newValue)
            {
                FieldName = fieldName;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        public class ListChange
        {
            public string ListName { get; set; }
            public string Action { get; set; }
            public string ListValues { get; set; }

            public ListChange(string listName, string action, string listValues)
            {
                ListName = listName;
                Action = action;
                ListValues = listValues;
            }
        }

    }
}