using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using FastMember;
using Npgsql;

namespace DAL.iMetal.Core.DbUtilities
{
    public static class ExtensionMethods
    {
        public static T ConvertToObject<T>(this NpgsqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    var member = members.FirstOrDefault(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));

                    if (member != null)
                    {
                        try
                        {
                            var value = rd.GetValue(i);
                            var memberName = member.Name;
                            accessor[t, memberName] = value;
                        }
                        catch (Exception e)
                        {
                            var exceptionError = $"{fieldName} error {e.Message}";
                            if (e.InnerException != null)
                            {
                                exceptionError += $":{e.InnerException.Message}";
                            }
                            Console.WriteLine(exceptionError);
                            throw new Exception(exceptionError);
                        }
                    }
                }
            }

            return t;
        }

        public static T ConvertToStruct<T>(this NpgsqlDataReader rd) where T : struct
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    var member = members.FirstOrDefault(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));

                    if (member != null)
                    {
                        var value = rd.GetValue(i);
                        var memberName = member.Name;
                        accessor[t, memberName] = value;
                    }
                }
            }

            return t;
        }

        public static ExpandoObject ConvertToDynamic(this NpgsqlDataReader rd)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);
                    var value = rd.GetValue(i);
                    dict.Add(fieldName,value);
                }
            }

            dynamic eo = dict.Aggregate(new ExpandoObject() as IDictionary<string, Object>,
                (a, p) => { a.Add(p.Key, p.Value); return a; });
            return eo;
        }

        public static Dictionary<string,object> ConvertToDictionary(this NpgsqlDataReader rd)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);
                    var value = rd.GetValue(i);
                    dict.Add(fieldName, value);
                }
            }

            return dict;
        }

    }
}
