using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace DAL.Vulcan.Mongo.Base.Static_Helper_Classes
{
    public static class ObjectDumper
    {
        public static string DumpAsYaml<T>(this T x)
        {
            var stringBuilder = new StringBuilder();
            var serializer = new Serializer();
            serializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), x);
            return stringBuilder.ToString();
        }

        public static string Dump<T>(this T x)
        {
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            return json;
        }
    }
}
