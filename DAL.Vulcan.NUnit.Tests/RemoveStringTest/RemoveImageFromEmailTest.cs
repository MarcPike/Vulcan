using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.RemoveStringTest
{
    [TestFixture]
    public class RemoveImageFromEmailTest
    {
        [Test]
        public void Test()
        {
            var body =
                @"To see if CC is retained\r\n\r\n________________________________\r\nMarc Pike | IT Developer I\r\n\r\n[cid:image001.png@01D33DB6.70DEFB00]\r\n\r\nHOWCO\r\n9611 Telge Rd | Houston, TX | 77095\r\nTel: 281-649-8954 | Fax: 281-649-8807\r\nmarc.pike@howcogroup.com<mailto:marc.pike@howcogroup.com>\r\n\r\n";
            body = RemoveImageFromBody(body);
            Console.WriteLine(body);

        }

        private string RemoveImageFromBody(string emailBody)
        {
            var result = emailBody;

            var indexOfImageStart = emailBody.IndexOf("[cid:", StringComparison.Ordinal);
            if (indexOfImageStart > -1)
            {
                var indexOfImageEnd = emailBody.IndexOf("]", indexOfImageStart, StringComparison.Ordinal);
                if (indexOfImageEnd > indexOfImageStart)
                {
                    result = result.Remove(indexOfImageStart, indexOfImageEnd - indexOfImageStart);
                }
            }
            return result;
        }

    }
}
