using System.IO;

namespace Console.Osiris.TagDump
{
    public static class CoidTagFolderBuilder
    {
        private static string RootFolder = @"G:\Shared drives\VulcanCRM Team Drive\TubeSupplyOsirisTagDump";

        public static void MakeFolder(CoidTag coidTag)
        {
            

            if (!Directory.Exists(coidTag.FolderName))
            {
                Directory.CreateDirectory(coidTag.FolderName);
            }

        }

        public static string GetFolderName(CoidTag coidTag)
        {
            var folderName = RootFolder + "\\" + coidTag.Coid + "-" + coidTag.TagNumber;
            return folderName;
        }

    }
}