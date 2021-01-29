using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BLL.Spreadsheet.ReadWrite
{
    [TestFixture()]
    public class ExcelToTable
    {

        [Test]
        public void UglyExample()
        {
            var fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\HRS Demographic Import.xlsx";
            var dataTable = ConvertFileToDataTable(fileName);
            foreach (DataRow dataTableRow in dataTable.Rows)
            {
                var row = string.Empty;
                var lastColumn = dataTable.Columns[dataTable.Columns.Count - 1];
                foreach (DataColumn dataTableColumn in dataTable.Columns)
                {
                    row += $"[{dataTableColumn.Caption}] == {dataTableRow[dataTableColumn].ToString()}";
                    if (dataTableColumn.Caption != lastColumn.Caption)
                    {
                        row += ", ";
                    }
                }
                Console.WriteLine(row);
            }

        }

        [Test]
        public void BasicExample()
        {
            var fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Example.xlsx";
            var dataTable = ConvertFileToDataTable(fileName);
            foreach (DataRow dataTableRow in dataTable.Rows)
            {
                var row = string.Empty;
                var lastColumn = dataTable.Columns[dataTable.Columns.Count - 1];
                foreach (DataColumn dataTableColumn in dataTable.Columns)
                {
                    row += $"[{dataTableColumn.Caption}] == {dataTableRow[dataTableColumn].ToString()}";
                    if (dataTableColumn.Caption != lastColumn.Caption)
                    {
                        row += ", ";
                    }
                }
                Console.WriteLine(row);
            }
        }

        public DataTable ConvertFileToDataTable(string fileName)
        {
            

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                ExcelDataReader.IExcelDataReader reader;

                // Create Reader - old until 3.4+
                ////var file = new FileInfo(originalFileName);
                ////if (file.Extension.Equals(".xls"))
                ////    reader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(stream);
                ////else if (file.Extension.Equals(".xlsx"))
                ////    reader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(stream);
                ////else
                ////    throw new Exception("Invalid FileName");
                // Or in 3.4+ you can only call this:
                reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                //// reader.IsFirstRowAsColumnNames
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                var dataSet = reader.AsDataSet(conf);
                return dataSet.Tables[0];

            }
        }
    }
}
