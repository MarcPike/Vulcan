using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class OemUpdateModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public void Validate()
        {
            var helperUser = new HelperUser();
            if (OldValue == NewValue) throw new Exception("No changes made");
            if (String.IsNullOrEmpty(OldValue)) throw new Exception("Old Value is empty");
            if (String.IsNullOrEmpty(NewValue)) throw new Exception("New Value is empty");

            var crmUser = helperUser.GetCrmUser(Application, UserId);
            var appPerm = new RepositoryBase<Permission>().AsQueryable().FirstOrDefault(x => x.Name == "CAN_UPDATE_OEM");
            if (appPerm == null) throw new Exception("Missing App permissions, looking for CAN_UPDATE_OEM");

            if (appPerm.Users.All(x => x.UserId != crmUser.UserId))
            {
                throw new Exception("You are not authorized to update Oem list");
            }

        }

        public static List<OemUpdateModel> GetAll(string application, string userId)
        {
            var result = new List<OemUpdateModel>();
            var oemList = new RepositoryBase<OemType>().AsQueryable().ToList();
            foreach (var oemType in oemList)
            {
                result.Add(new OemUpdateModel()
                {
                    Application = application,
                    UserId = userId,
                    OldValue = oemType.Name,
                    NewValue = oemType.Name
                });
            }

            return result;
        }

        public static (int RowsAffected, bool OemTypeWillBeRemoved) PreCheck(OemUpdateModel model)
        {
            model.Validate();
            var rowsAffected = 0;
            var oemTypeWillBeRemoved = false;
            var rep = new RepositoryBase<OemType>();
            var oldOem = rep.AsQueryable().FirstOrDefault(x => x.Name == model.OldValue);
            if (oldOem == null) throw new Exception("Invalid Oem value, OldValue does not exist");

            var newOem = rep.AsQueryable().FirstOrDefault(x => x.Name == model.NewValue);
            if (newOem != null) oemTypeWillBeRemoved = true;

            rowsAffected = new RepositoryBase<CrmQuoteItem>().AsQueryable().Count(x=>x.OemType == model.OldValue);

            return (rowsAffected, oemTypeWillBeRemoved);
        }

        public static List<OemUpdateModel> Execute(OemUpdateModel model)
        {

            model.Validate();
            var rep = new RepositoryBase<OemType>();
            var oldOem = rep.AsQueryable().FirstOrDefault(x => x.Name == model.OldValue);
            if (oldOem == null) throw new Exception("Invalid Oem value, OldValue does not exist");

            var existingOem = rep.AsQueryable().FirstOrDefault(x => x.Name == model.NewValue);
            if (existingOem != null)
            {
                rep.RemoveOne(oldOem);
            }
            else
            {
                oldOem.Name = model.NewValue;
                rep.Upsert(oldOem);
            }

            var rowsAffected = new RepositoryBase<CrmQuoteItem>().AsQueryable().Where(x => x.OemType == model.OldValue).ToList();
            foreach (var crmQuoteItem in rowsAffected)
            {
                crmQuoteItem.OemType = model.NewValue;
                crmQuoteItem.SaveToDatabase();
            }

            return GetAll(model.Application, model.UserId);
        }

    }
}
