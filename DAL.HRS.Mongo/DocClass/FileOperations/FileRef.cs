using System;
using System.IO;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;

namespace DAL.HRS.Mongo.DocClass.FileOperations
{
    public class FileRef
    {
        public string FileName { get; set; }
        public string SourcePath { get; set; }
        public DateTime FileCreateDate { get; set; }
        public DateTime UploadDate { get; set; }
        public HrsUserRef UploadedByUser { get; set; }

        public FileRef(string fullFileName, DateTime fileCreateDate, HrsUserRef uploadedByUser)
        {
            FileName = Path.GetFileName(fullFileName);
            SourcePath = Path.GetFullPath(fullFileName);
            FileCreateDate = fileCreateDate;
            UploadDate = DateTime.Now;
            UploadedByUser = uploadedByUser;
        }

        public string FileNameObfuscated()
        {
            var helperSecurity = new HelperSecurity();
            var fileName = Path.GetFileNameWithoutExtension(FileName);
            var extension = Path.GetExtension(FileName);
            return $"{helperSecurity.EncodeToBase64(fileName)}.{helperSecurity.EncodeToBase64(extension)}";
        }



    }
}