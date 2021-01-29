using System;
using System.Collections.Generic;
using System.Text;
using DAL.iMetal.Core.Context;
using NUnit.Framework;

namespace DAL.iMetal.Core.Tests.ContextFactoryTest
{
    [TestFixture]
    class CreateConnection
    {
        [Test]
        public void GoForIt()
        {
            ConnectionFactory.Initialize();
        }
    }
}
