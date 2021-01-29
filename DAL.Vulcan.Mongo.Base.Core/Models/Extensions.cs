using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastMember;

namespace DAL.Vulcan.Mongo.Base.Core.Models
{
    public static class ExtensionMethods
    {
        //public static T ConvertToObject<T>(this object source) where T : class, new()
        //{
        //    Type type = typeof(T);
        //    var accessor = TypeAccessor.Create(type);
        //    var members = accessor.GetMembers();
        //    var t = new T();

        //    Type sourceType = typeof(T);
        //    var sourceAccessor = TypeAccessor.Create(sourceType);
        //    var sourceMembers = sourceAccessor.GetMembers();

        //    foreach (var member in members)
        //    {
        //        var sourceField = sourceMembers.FirstOrDefault(x => x.Name == member.Name && x.Type == member.Type);
        //        if (sourceField != null)
        //        {
        //            var value = sourceField.;
        //            var memberName = member.Name;
        //            accessor[t, memberName] = value;

        //        }
        //    }

        //    return t;
        //}
    }
}
