using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Base.Core.Validators
{
    public class BaseValidator<TBaseDocument>
    {
        public virtual List<ValidationError> Validate(TBaseDocument doc)
        {
            return new List<ValidationError>();
        }

    }
}
