using System;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public static class GuidGenerator
    {
        public static string AsString()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();
            return Convert.ToBase64String(bytes);
        }
    }
}
