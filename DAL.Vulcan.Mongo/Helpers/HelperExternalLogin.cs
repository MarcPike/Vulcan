using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.ExternalLogin;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperExternalLogin : IHelperExternalLogin
    {
        public ExternalLoginModel Login(string userName, string password)
        {
            var queryHelper = new MongoRawQueryHelper<ExternalLogin>();
            var filter = queryHelper.FilterBuilder.Where(x => x.UserName == userName && x.Password == password);

            var rowFound = queryHelper.Find(filter).FirstOrDefault();

            if (rowFound == null) throw new Exception("Invalid login");

            return new ExternalLoginModel(rowFound, string.Empty);
        }

        public ExternalLoginModel GetNewExternalLogin(string adminUserId)
        {
            var queryHelper = new MongoRawQueryHelper<ExternalLogin>();
            return new ExternalLoginModel(new ExternalLogin(), adminUserId);
        }


        public ExternalLoginModel SaveExternalLogin(ExternalLoginModel model)
        {
            var queryHelper = new MongoRawQueryHelper<ExternalLogin>();
            var rowFound = queryHelper.Find(queryHelper.FilterBuilder.Where(x => x.Id.ToString() == model.Id)).FirstOrDefault();
            if (rowFound == null)
            {
                var rowFoundWithSameName = queryHelper
                    .Find(queryHelper.FilterBuilder.Where(x => x.UserName == model.UserName)).FirstOrDefault();
                if (rowFoundWithSameName != null)
                {
                    throw new Exception("User name already in use");
                }
                rowFound = new ExternalLogin()
                {
                    Id = ObjectId.Parse(model.Id)
                };
            }

            rowFound.UserName = model.UserName;
            rowFound.Password = model.Password;
            rowFound.SaveToDatabase();

            return new ExternalLoginModel(rowFound, model.AdminUserId);

        }

        public List<ExternalLoginModel> RemoveExternalLogin(string externalLoginId, string adminUserId)
        {
            var queryHelper = new MongoRawQueryHelper<ExternalLogin>();
            var rowFound = queryHelper.Find(queryHelper.FilterBuilder.Where(x => x.Id.ToString() == externalLoginId)).FirstOrDefault();
            if (rowFound != null) new RepositoryBase<ExternalLogin>().RemoveOne(rowFound);

            return GetAllExternalLogins(adminUserId);

        }

        public List<ExternalLoginModel> GetAllExternalLogins(string adminUserId)
        {
            return new RepositoryBase<ExternalLogin>().AsQueryable()
                .OrderBy(x => x.UserName).ToList()
                .Select(x => new ExternalLoginModel(x, adminUserId)).ToList();
        }
    }
}
