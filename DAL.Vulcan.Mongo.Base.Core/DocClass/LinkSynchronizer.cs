using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public static class LinkSynchronizer<TBaseDocumentLinkType>
        where TBaseDocumentLinkType : BaseDocument
    {
        public static void SynchronizeWithModelValues(BaseDocument document, List<TBaseDocumentLinkType> modelValues, bool oneWay = false)
        {
            var existingLinks = new LinkResolver<TBaseDocumentLinkType>(document).GetAllLinkedDocuments();
            foreach (var value in modelValues)
            {
                if (existingLinks.All(x => x.Id != value.Id))
                {
                    if (oneWay)
                    {
                        document.CreateLink(value);
                    }
                    else
                    {
                        document.CreateLinkOneWay(value);
                    }
                }
            }
            foreach (var existingLink in existingLinks)
            {
                if (modelValues.All(x => x.Id != existingLink.Id))
                {
                    document.RemoveLink(existingLink);
                }
            }
        }
    }
}