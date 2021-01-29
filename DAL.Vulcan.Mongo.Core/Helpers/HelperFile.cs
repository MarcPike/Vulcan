using System;
using System.Collections.Generic;
using System.IO;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperFile : HelperBase, IHelperFile
    {
        public Dictionary<string,string> MimiTypes = new Dictionary<string, string>()
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };

        public HelperFile()
        {
        }

        public string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (ext == null) throw new Exception("FileType is not supported");
            return MimiTypes[ext];
        }
    }
}