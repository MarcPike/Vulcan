using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.Repository;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public class LinkResolver<TBaseDocumentLinkType>
        where TBaseDocumentLinkType : BaseDocument
    {
        private readonly BaseDocument _document;
        public LinkResolver(BaseDocument doc)
        {
            _document = doc;
        }

        private List<TBaseDocumentLinkType> GetDocumentsFromLinks(List<Link> list)
        {
            var rep = new RepositoryBase<TBaseDocumentLinkType>();
            var result = new List<TBaseDocumentLinkType>();
            var type = typeof(TBaseDocumentLinkType);
            foreach (var link in list.Where(x => x.TypeFullName == type.FullName).ToList())
            {
                var rowFound = rep.AsQueryable().SingleOrDefault(x => x.Id == link.Id);
                if (rowFound != null) result.Add(rowFound);
            }
            return result;
        }

        public List<Link> GetAllLinks()
        {
            return _document.Links.Where(x => x.TypeFullName == typeof(TBaseDocumentLinkType).FullName).ToList();
        }

        public List<TBaseDocumentLinkType> GetAllLinkedDocuments()
        {
            return GetDocumentsFromLinks(_document.Links.Where(x => x.TypeFullName == typeof(TBaseDocumentLinkType).FullName).ToList());
        }

        public void RemoveAllLinksForThisType()
        {
            foreach (var linkedDocument in GetDocumentsFromLinks(_document.Links.Where(x => x.TypeFullName == typeof(TBaseDocumentLinkType).FullName).ToList())) 
            {
                linkedDocument.RemoveLink(_document);
            }
        }
    }
}
