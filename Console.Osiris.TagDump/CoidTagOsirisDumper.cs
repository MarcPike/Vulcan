using System.IO;
using System.Linq;
using DAL.Osiris;
using System;

namespace Console.Osiris.TagDump
{
    public class CoidTagOsirisDumper
    {
        public CoidTag CoidTag;
        public CoidTagOsirisDumper(CoidTag coidTag)
        {
            CoidTag = coidTag;
            
        }

        public void Execute()
        {
            var repository = new OsirisRepository();
            var docs = repository.GetOsirisDocumentList(CoidTag.Coid, CoidTag.TagNumber, "");

            if (!docs.Any())
            {
                return;
            }
            
            CoidTag.OsirisDocs = docs;

      
            
            foreach (var osirisDocInfo in docs)
            {
                var memoryStream = repository.GetOsirisDocumentAsStream(CoidTag.Coid, osirisDocInfo.ID);
                memoryStream.Position = 0;
                var fileName = CoidTag.FolderName+ "/" + osirisDocInfo.FileName.Replace(@"/","-");

                using (FileStream file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[memoryStream.Length];
                    memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    memoryStream.Close();
                }
            }

        }
    }
}