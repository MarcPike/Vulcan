using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;

namespace Console.Osiris.TagDump
{
    class Program
    {
        static void Main(string[] args)
        {
            //var coidTagList = GenerateFiles();

            var coidTagList = CoidTagList.GetCoidTagList();
            foreach (var coidTag in coidTagList.TagList)
            {
                coidTag.CountFiles();
            }

            var tagsWithNoFiles = coidTagList.TagList.Where(x => x.FileCount == 0).OrderBy(x => x.FolderName).ToList();
            foreach (var coidTag in tagsWithNoFiles)
            {
                var dumper = new CoidTagOsirisDumper(coidTag);
                dumper.Execute();
            }

            EmailResults(coidTagList);
        }

        private static CoidTagList GenerateFiles()
        {
            var startPositionReached = false;
            var coidTagList = CoidTagList.GetCoidTagList();
            foreach (var coidTag in coidTagList.TagList)
            {
                coidTag.CountFiles();
            }

            System.Console.WriteLine(coidTagList.TagList.Count(x => x.FileCount == 0) + " missing files");

            var onRow = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();


            foreach (var coidTag in coidTagList.TagList.OrderBy(x => x.FolderName).ToList())
            {
                ++onRow;
                if (coidTag.TagNumber == @"1335925A")
                {
                    startPositionReached = true;
                }

                if ((onRow % 500) == 0)
                {
                    System.Console.WriteLine($"On Row: " + onRow + " Elapsed: " + sw.Elapsed);
                }

                if (!startPositionReached) continue;
                var dumper = new CoidTagOsirisDumper(coidTag);
                dumper.Execute();
            }

            return coidTagList;
        }

        private static void EmailResults(CoidTagList coidTagList)
        {
            foreach (var coidTag in coidTagList.TagList)
            {
                coidTag.CountFiles();
            }

            var body = new StringBuilder();

            body.AppendLine(
                @"Folder Link: https://drive.google.com/drive/folders/1UGSQDSxT_jdGuNERCpZqunzihOT4A_7A?usp=sharing");
            body.AppendLine();

            System.Console.WriteLine("=========================");
            System.Console.WriteLine("Tags with no files found ");
            System.Console.WriteLine("=========================");
            var tagsWithNoFiles = coidTagList.TagList.Where(x => x.FileCount == 0).OrderBy(x => x.FolderName).ToList();
            foreach (var coidTag in tagsWithNoFiles)
            {
                body.AppendLine(coidTag.Coid +"-"+coidTag.TagNumber);
            }

            var recipients = new List<string>()
            {
                "stanton.fraser@howcogroup.com",
                "rebecca.levine@howcogroup.com",
                "marc.pike@howcogroup.com"
            };

            BLL.EMail.SendEMail.Execute(
                "Tube Supply Tags with no documents",
                body.ToString(), recipients, "marc.pike@howcogroup.com",
                false);
        }
    }
}
