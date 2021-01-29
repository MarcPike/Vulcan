using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Common.DocClass
{
    public interface ISupportLocationNameChanges
    {
        void ChangeOfficeName(string locationId, string newName);
    }
}