using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.QNG.Exporter
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
            string msg = "";

            foreach (var eve in dbve.EntityValidationErrors)
            {
                msg += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\r\n\r\n", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    msg += string.Format("- Property: \"{0}\", Error: \"{1}\"\r\n", ve.PropertyName, ve.ErrorMessage);
                }
            }

            return msg;
        }


    }
}
