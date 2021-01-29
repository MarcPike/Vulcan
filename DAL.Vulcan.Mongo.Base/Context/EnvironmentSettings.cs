

using System;

namespace DAL.Vulcan.Mongo.Base.Context
{
    public static class EnvironmentSettings
    {
        public static Environment CurrentEnvironment { get; set; } = Environment.Undefined;
        public static SecurityType SecurityType { get; set; } = SecurityType.Undefined;
        public static MongoDatabase Database { get; set; } = MongoDatabase.Undefined;

        public static bool RunningLocal { get; set; } = false;

        public static void BiDevelopment()
        {
            CurrentEnvironment = Environment.Development;
            SecurityType = SecurityType.BiUser;
            Database = MongoDatabase.VulcanBI;
        }

        public static void BiQuality()
        {
            CurrentEnvironment = Environment.QualityControl;
            SecurityType = SecurityType.BiUser;
            Database = MongoDatabase.VulcanBI;
        }

        public static void BiProduction()
        {
            CurrentEnvironment = Environment.Production;
            SecurityType = SecurityType.BiUser;
            Database = MongoDatabase.VulcanBI;
        }

        public static void HrsDevelopment()
        {
            CurrentEnvironment = Environment.Development;
            SecurityType = SecurityType.HrsUser;
            Database = MongoDatabase.VulcanHrs;
        }

        public static void HrsQualityControl()
        {
            CurrentEnvironment = Environment.QualityControl;
            SecurityType = SecurityType.HrsUser;
            Database = MongoDatabase.VulcanHrs;
        }

        public static void HrsProduction()
        {
            CurrentEnvironment = Environment.Production;
            SecurityType = SecurityType.HrsUser;
            Database = MongoDatabase.VulcanHrs;
        }

        public static void CrmDevelopment()
        {
            CurrentEnvironment = Environment.Development;
            SecurityType = SecurityType.SalesPerson;
            Database = MongoDatabase.VulcanCrm;
        }

        public static void CrmProduction()
        {
            CurrentEnvironment = Environment.QualityControl;
            SecurityType = SecurityType.SalesPerson;
            Database = MongoDatabase.VulcanCrm;
        }

        public static string GetBaseAddress()
        {
            if (EnvironmentSettings.Database == MongoDatabase.VulcanCrm)
            {
                if (EnvironmentSettings.RunningLocal)
                {
                    return "http://192.168.102.223:5000";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                {
                    return "http://s-us-vapi01.howcogroup.com:80";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                {
                    return "http://s-us-vapiqc.howcogroup.com:80";
                }

                {
                    return "http://192.168.102.223:5000";
                }
            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanHrs)
            {
                if (EnvironmentSettings.RunningLocal)
                {
                    return "http://192.168.102.223:5001";
                    //return "http://10.211.55.4:59484";
                    //return "http://10.05.30.27:59484";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                {
                    return "http://s-us-hrsdev01.howcogroup.com:80";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                {
                    return "http://s-us-hrsqc01.howcogroup.com:80";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Production)
                {
                    return "http://s-us-hrs01.howcogroup.com:80";
                }
                else
                {
                    return "http://10.211.55.4:59484";
                }

            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanBI)
            {
                if (EnvironmentSettings.RunningLocal)
                {
                    return "http://192.168.102.223:5002";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                {
                    return "http://s-us-bidev01.howcogroup.com:80";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                {
                    return "http://s-us-biqc01.howcogroup.com:80";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Production)
                {
                    return "http://s-us-bi01.howcogroup.com:80";
                }
                else
                {
                    return "http://192.168.102.223:5002";
                }

            }
            else
            {
                throw new Exception("Database not specified");
            }


        }

        public static object GetWebAddress()
        {
            if (EnvironmentSettings.Database == MongoDatabase.VulcanCrm)
            {
                if (EnvironmentSettings.RunningLocal)
                {
                    return "http://s-us-web02:82";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                {
                    return "http://s-us-web02:82";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                {
                    return "http://s-us-web02:88";
                }

                {
                    return "http://s-us-web02:82";
                }
            }
            else
            {
                if (EnvironmentSettings.RunningLocal)
                {
                    return "http://s-us-web02:90";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                {
                    return "http://s-us-web02:90";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                {
                    return "http://s-us-web02:90";
                }
                else if (EnvironmentSettings.CurrentEnvironment == Environment.Production)
                {
                    return "http://s-us-web02:90";
                }
                else
                {
                    return "http://s-us-web02:90";
                }

            }
        }

    }
}