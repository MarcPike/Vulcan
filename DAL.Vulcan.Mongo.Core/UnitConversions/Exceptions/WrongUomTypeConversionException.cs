using System;

namespace DAL.Vulcan.Mongo.Core.UnitConversions.Exceptions
{
    public class WrongUomTypeConversionException : Exception
    {
        public override string Message => "Wrong UomType(s) to convert";
    }
}