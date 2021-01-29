using System.Data.Entity.Validation;
using System.Linq;

namespace Vulcan.iMetal.Quote.Export.Helpers
{
    public static class EF_Utils
    {
        /// <summary>
        /// Gets a composite entity validation error.
        /// </summary>
        /// <param name="dbve"></param>
        /// <returns></returns>
        public static string GetEntityValidationErrors(DbEntityValidationException dbve)
        {
            var msg = "";

            foreach (var eve in dbve.EntityValidationErrors)
            {
                msg += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\r\n\r\n";
                msg = eve.ValidationErrors.Aggregate(msg, (current, ve) => current + $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\r\n");
            }

            return msg;
        }
    }
}
