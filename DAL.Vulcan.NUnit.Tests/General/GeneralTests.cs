using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.General
{
    [TestFixture]
    public class GeneralTests
    {

        [Test]
        public void StringTest()
        {
            var testValue = "five";
            var answer = ConvertSpecialCharactersToHtml(testValue);
            Console.WriteLine(answer);


        string ConvertSpecialCharactersToHtml(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            input = Encoding.UTF8.GetString(bytes);

            var result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                var thisChar = input.Substring(i, 1);
                if (thisChar == "\"") // Why is this not working
                {
                    result += "&quot;";
                }
                else if (thisChar == "\'")
                {
                    result += "&apos;";
                }
                else
                {
                    result += thisChar;
                }
            }

            return result;
        }

    }

    }
}
