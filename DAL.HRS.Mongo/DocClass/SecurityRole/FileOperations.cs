namespace DAL.HRS.Mongo.DocClass.SecurityRole
{
    public class FileOperations
    {
        public bool CanUpload { get; set; } = false;
        public bool CanDownload { get; set; } = false;
        public bool CanDelete { get; set; } = false;

        public void AllAccess()
        {
            CanUpload = true;
            CanDownload = true;
            CanDelete = true;
        }
        public void ReadOnly()
        {
            CanUpload = false;
            CanDownload = true;
            CanDelete = false;
        }

        public void ReadWrite()
        {
            CanUpload = true;
            CanDownload = true;
            CanDelete = false;
        }
        public void ReadWriteDelete()
        {
            CanUpload = true;
            CanDownload = true;
            CanDelete = true;
        }

    }
}