using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class FileUploadModel
    {
        public string BaseDocumentId { get; set; }
        public bool IsHrs { get; set; }
        public string ModuleName { get; set; }
        public string DocumentType { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DocumentDate { get; set; } = DateTime.Now;
        public string FileName { get; set; }
        public string Comments { get; set; }
        //public IFormFile File { get; set; }
    }
}
