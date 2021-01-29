using System;

namespace DAL.Vulcan.Mongo.UnitConversions.Exceptions
{
    public class DuplicateUomException : Exception
    {
        public override string Message => "Duplicate Name. Uom already exists.";
    }
}