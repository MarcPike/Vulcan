using System.Collections.Generic;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public struct EnumValue
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<EnumValue> FromEnum(string type)
        {
            var result = new List<EnumValue>();
            if (type == "StringFilterType")
            {
                result.Add(new EnumValue() { Id = 0, Name = "Contains"});
                result.Add(new EnumValue() { Id = 1, Name = "StartsWith"});
                result.Add(new EnumValue() { Id = 2, Name = "EqualTo" });
                result.Add(new EnumValue() { Id = 3, Name = "InList" });
            }

            return result;
        }
    }
}