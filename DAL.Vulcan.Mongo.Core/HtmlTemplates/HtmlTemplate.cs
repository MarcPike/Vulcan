using System;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Core.HtmlTemplates
{
    public class HtmlTemplate: BaseDocument
    {
        public string Coid { get; set; } = String.Empty;
        public string Name { get; set; }
        public string Html { get; set; }

        public HtmlTemplate()
        {
        }

        public HtmlTemplate(string coid, string name, string fileName)
        {
            Coid = coid;
            Name = name;
            Html = System.IO.File.ReadAllText(@fileName);
        }

        public void UpdateFromFile(string fileName)
        {
            Html = System.IO.File.ReadAllText(@fileName);
        }
    }
}