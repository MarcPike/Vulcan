using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.HtmlTemplates;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class HtmlTemplateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string Application { get; set; }
        public string UserId { get; set; }
        public HtmlTemplateModel()
        {
        }
        public HtmlTemplateModel(HtmlTemplate template, string application, string userId)
        {
            Id = template.Id.ToString();
            Name = template.Name;
            Html = template.Html;
            Application = application;
            UserId = userId;
        }

    }
}
