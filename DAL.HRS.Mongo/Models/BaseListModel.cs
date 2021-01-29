using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class BaseListModel
    {
        protected void LoadCorrectPropertyValueRefs(EntityRef entity, List<PropertyValueRef> props)
        {
            if (entity == null) return;
            foreach (var propertyValueRef in props)
            {
                if (propertyValueRef != null)
                {
                    propertyValueRef.Load(entity);
                }
            }
        }

        protected void LoadCorrectPropertyValueRef(EntityRef entity, PropertyValueRef prop)
        {
            if (entity == null) return;
            if (prop == null) return;
            prop.Load(entity);
        }


        public bool IsDirty { get; set; } = false;
    }
}