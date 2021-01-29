using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.MssService.Client
{
    public static class ConsoleUtil
    {
        public static string GetStringValue(string prompt)
        {
            Console.WriteLine($"Enter a [{prompt}]:");
            var input = Console.ReadLine();
            return input;
        }

        public static int GetIntegerValue(string prompt, int defaultValue)
        {
            Console.WriteLine($"Enter a [{prompt}]:");
            var input = Console.ReadLine();
            int result = 0;
            try
            {
                if (input == string.Empty)
                {
                    result = defaultValue;
                }
                else
                {
                    result = Convert.ToInt32(input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"    -> Invalid input so using a {defaultValue}");
                result = defaultValue;
            }
            return result;
        }

        public static bool GetBoolValue(string prompt)
        {
            Console.WriteLine($"{prompt}? (Y == Yes  or anything else is No):");
            var input = Console.ReadLine();
            bool result = input.ToUpper() == "Y";
            return result;
        }

    }
}
