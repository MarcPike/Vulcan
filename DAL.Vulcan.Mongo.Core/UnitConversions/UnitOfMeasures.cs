using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.UnitConversions.Exceptions;

namespace DAL.Vulcan.Mongo.Core.UnitConversions
{
    public class UnitOfMeasures : List<BaseUnitOfMeasure>
    {
        public void RegisterUom(BaseUnitOfMeasure uom)
        {
            CheckForDuplicate(uom);
            CheckOnlyOneBaseUnitCanHaveFactorEqualToOne(uom);
            this.Add(uom);
        }

        public BaseUnitOfMeasure FindByName(UomType type, string name)
        {
            return Find(x => x.UomType == type && x.Name == name);
        }

        public BaseUnitOfMeasure GetBaseUnitOfMeasure(UomType type)
        {
            return Find(x => (x.UomType == type) && (x.GetBaseUnitFactor() == (decimal) 1));
        }

        private void CheckOnlyOneBaseUnitCanHaveFactorEqualToOne(BaseUnitOfMeasure uom)
        {
            if (uom.GetBaseUnitFactor() != (decimal)1) return;

            if (GetBaseUnitOfMeasure(uom.UomType) == null)
                return;

            throw new OnlyOneBaseUnitCanHaveFactorEqualToOneException();
        }

        private void CheckForDuplicate(BaseUnitOfMeasure uom)
        {
            if (this.Any(x => x.Name == uom.Name && x.UomType == uom.UomType)) throw new DuplicateUomException();
        }

        public void Deregister(BaseUnitOfMeasure uom)
        {
            var uomFound = Find(x => x.Name == uom.Name);

            if (uomFound != null)
                Remove(uomFound);
        }

        public UnitOfMeasures()
        {
            
        }

    }
}
