using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class BaseModel
    {
        protected Encryption _encryption = Encryption.NewEncryption;


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

    }
}
