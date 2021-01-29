using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public static class SystemModuleFactory
    {
        /*
        public static SystemModule CreateAndCloneHrsModule(string name)
        {
            var module = new RepositoryBase<SystemModule>().AsQueryable()
                .SingleOrDefault(x => x.ModuleType.Name == name && x.ModuleType.IsHrsModule);
            if (module != null) return SystemModule.Clone(module);
            else
            {
                return new SystemModule()
                {
                    ModuleType = module.ModuleType,
                };
            }
        }

        public static SystemModule CreateAndCloneHseModule(string name)
        {
            var module = new RepositoryBase<SystemModule>().AsQueryable()
                .SingleOrDefault(x => x.Name == name && x.HseModule);
            if (module != null) return SystemModule.Clone(module);
            else
            {
                return new SystemModule()
                {
                    Name = name,
                    HrsModule = true
                };
            }
        }
        */
    }
}
