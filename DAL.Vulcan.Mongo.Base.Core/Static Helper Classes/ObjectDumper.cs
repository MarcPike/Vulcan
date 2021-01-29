using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DAL.Vulcan.Mongo.Base.Core.Static_Helper_Classes
{
    public static class ObjectDumper
    {

        public static string Dump<T>(this T x)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var jsonModel = JsonSerializer.Serialize<T>(x, options);
            return jsonModel;
        }
    }
}
