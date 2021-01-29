using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.MathTest
{
    [TestFixture()]
    public class PageCountTest
    {
        [Test]
        public void Execute()
        {
            int LineItems = 125;
            int LinesPerPage = 5;

            var pages = (LineItems / LinesPerPage) + 1;

            Console.WriteLine(pages);
        }
    }
}
