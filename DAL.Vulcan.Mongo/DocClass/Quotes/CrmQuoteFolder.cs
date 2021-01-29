using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class CrmQuoteFolder : BaseDocument
    {
        public CrmQuoteFolder Parent { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public List<CrmQuoteFolder> Children { get; set; } = new List<CrmQuoteFolder>();
    }

    public class SalesPersonFolder : CrmQuoteFolder
    {
        public CrmUserRef SalesPerson { get; set; }

        public SalesPersonFolder()
        {

        }

        public static SalesPersonFolder CreateNew(CrmUserRef salesPerson, SalesPersonFolder parent, string name)
        {
            var result = new SalesPersonFolder()
            {
                Name = name,
                Parent = parent,
                SalesPerson = salesPerson
            };
            return result;
        }
    }

    public class SalesPersonFolderModel
    {
        public string Application { get; set; } 
        public string UserId { get; set; }
        public List<SalesPersonFolder> RootFolders;

        public SalesPersonFolderModel(string application, string userId, CrmUserRef salesPerson)
        {
            Application = application;
            UserId = userId;
            RootFolders = new RepositoryBase<SalesPersonFolder>().AsQueryable()
                .Where(x => x.SalesPerson.UserId == salesPerson.UserId && x.Parent == null).ToList();
        }
    }
}
