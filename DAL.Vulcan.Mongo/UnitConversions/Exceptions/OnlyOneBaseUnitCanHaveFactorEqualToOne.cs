using System;

namespace DAL.Vulcan.Mongo.UnitConversions.Exceptions
{
    public class OnlyOneBaseUnitCanHaveFactorEqualToOneException : Exception
    {
        public override string Message => "Only one Uom can have a BaseFactor equal to 1 for a UomType";
    }
}