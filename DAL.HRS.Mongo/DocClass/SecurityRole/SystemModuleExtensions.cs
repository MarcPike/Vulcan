using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public static class SystemModuleExtensions
    {
        public static SystemModuleModel GetModel(this SystemModule module)
        {
            return new SystemModuleModel(module);
        }
    }
}
