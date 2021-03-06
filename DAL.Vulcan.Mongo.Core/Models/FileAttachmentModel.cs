﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL.Quotes.Mongo.Core.DocClass.FileAttachment;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class FileAttachmentModel
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UploadDateTime { get; set; } 

        public string MimeType { get; set; } = String.Empty;

        public bool OsirisDocument { get; set; } = false;
        public string OsirisDocType { get; set; } = string.Empty;
        public string OsirisDescription { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public FileAttachmentModel(GridFSFileInfo fileInfo)
        {
            Id = fileInfo.Id.ToString();
            FileName = fileInfo.Filename;
            UploadDateTime = fileInfo.UploadDateTime;
            MimeType = GetContentType(fileInfo.Filename);
            OsirisDocument = (fileInfo.Metadata.GetValue("fileAttachmentType") == FileAttachmentType.Osiris);
            if (OsirisDocument)
            {
                OsirisDocType = fileInfo.Metadata.GetValue("osirisDocType")?.ToString();
                OsirisDescription = fileInfo.Metadata.GetValue("osirisDocDescription")?.ToString();
            }

            FileSize = fileInfo.Length;
        }

        public FileAttachmentModel()
        {

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (types.All(x => x.Key != ext))
            {
                return "text/plain";
            }
            return types.Single(x => x.Key == ext).Value;
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".eml", "message/rfc822 eml"}
            };

        }

    }
}