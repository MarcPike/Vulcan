//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using BI.DAL.DataWarehouse.Core.Helpers;
//using BI.DAL.Mongo;
//using BI.DAL.Mongo.Helpers;
//using BI.DAL.Mongo.Models;
//using BI.DAL.Mongo.Security;

//namespace BI.WebApi.Tests
//{
//    public static class StaticTests
//    {
//        public static void GetBranchTest()
//        {
//            var helperOds = new HelperOds();
//            var coidList = "INC,CAN".Split(",").ToList();
//            var result = helperOds.GetBranchList(coidList);

//        }

//        public static BiUserToken Authenticate()
//        {
//            var helperSecurity = new HelperSecurity();
//            var networkId = helperSecurity.EncodeToBase64("mpike");
//            var password = helperSecurity.EncodeToBase64("funLove66/");
//            var result = helperSecurity.Authenticate(networkId, password);
//            return result;
//        }

//        public static BiUserModel GetBiUserModel()
//        {
//            var token = Authenticate();
//            var helperSecurity = new HelperSecurity();
//            var biUserModel = helperSecurity.GetBiUserModel(token.User.Id);
//            Console.WriteLine(ObjectDumper.Dump(biUserModel));
//            return biUserModel;
//        }


//    }
//}
