using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Osiris;

namespace Console.Osiris.TagDump
{
    public class CoidTag
    {
        public string Coid { get; set; }
        public string TagNumber { get; set; }

        public string FolderName => 
            @"G:\Shared drives\VulcanCRM Team Drive\TubeSupplyOsirisTagDump\"+Coid+"-"+TagNumber;

        public List<OsirisDocInfo> OsirisDocs = new List<OsirisDocInfo>();

        public int FileCount { get; set; }
        public void CountFiles()
        {
            FileCount = Directory.GetFiles(FolderName).Length;

}       

        public CoidTag()
        {
        }

        public CoidTag(string coid, string tagNumber)
        {
            Coid = coid;
            TagNumber = tagNumber;
            CoidTagFolderBuilder.MakeFolder(this);
        }
    }
}
