using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.TestStaticMemberInheritance
{
    public class SomeDumbClass
    {
        public RepositoryBase<Notification> Repository = new RepositoryBase<Notification>();

        public SomeDumbClass()
        {
            
        }
    }

    public class DumbBaseClass
    {
        public static RepositoryBase<Notification> Repository = new RepositoryBase<Notification>();
    }

    public class DumbConcreteClassOne : DumbBaseClass
    {
        public RepositoryBase<Notification> GetRepository()
        {
            return DumbBaseClass.Repository;
        }
    }

    public class DumbConcreteClassTwo : DumbBaseClass
    {
        public RepositoryBase<Notification> GetRepository()
        {
            return DumbBaseClass.Repository;
        }
    }

    [TestFixture]
    public class RunMe
    {
        [Test]
        public void Execute()
        {
            var class1 = new DumbConcreteClassOne();
            var class2 = new DumbConcreteClassTwo();

            var rep1 = class1.GetRepository();
            var rep2 = class2.GetRepository();

            Assert.AreSame(rep1,rep2);
        }
    }
}
