using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Base.DateTimeUtils
{
    public static class DateTimeUtilities
    {
        public static DateTime OffsetDateTimeForCoid(string coid, DateTime dateTime)
        {
            var sourceTimeZone = TimeZoneInfo.Local;
            var destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            if (coid == "INC")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            }
            if (coid == "EUR")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            }
            if (coid == "CAN")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Canada Central Standard Time");
            }
            if (coid == "SIN")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            if (coid == "MSA")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            if (coid == "DUB")
            {
                destTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
            }

            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destTimeZone);

        }

        public static CultureInfo GetCultureInfoForCoid(string coid)
        {
            if (coid == "INC")
            {
                return new CultureInfo("en-US");
            }

            if (coid == "CAN")
            {
                return new CultureInfo("en-CA");
            }

            if (coid == "EUR")
            {
                return new CultureInfo("en-GB");
            }

            if (coid == "SIN")
            {
                return new CultureInfo("en-US");
            }

            if (coid == "MSA")
            {
                return new CultureInfo("en-US");
            }

            if (coid == "DUB")
            {
                return new CultureInfo("en-US");
            }

            return new CultureInfo("en-US");

        }

    }
}
