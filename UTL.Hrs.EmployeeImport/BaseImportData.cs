using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Spreadsheet.ReadWrite;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace UTL.Hrs.EmployeeImport
{
    public class BaseImportData
    {
        public string FileName { get; set; }
        public DataTable Data { get; set; }

        public BaseImportData(string fileName)
        {
            FileName = fileName;
            var converter = new ExcelToTable();
            Data = converter.ConvertFileToDataTable(fileName);
        }

        public BaseImportData(DataTable dt)
        {
            Data = dt;
        }

        public virtual void Execute()
        {

        }

        public virtual void Execute(HrsUserRef hrsUser)
        {

        }
    }
}
