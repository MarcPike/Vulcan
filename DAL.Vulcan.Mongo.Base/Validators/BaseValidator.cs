using System.Collections.Generic;
using AspNetCore.Identity.MongoDB.Validators;

namespace AspNetCore.Identity.MongoDB.Validators
{
    public class BaseValidator<TBaseDocument>
    {
        public virtual List<ValidationError> Validate(TBaseDocument doc)
        {
            return new List<ValidationError>();
        }

    }
}
