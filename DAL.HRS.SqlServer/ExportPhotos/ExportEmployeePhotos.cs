using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.SqlServer.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.SqlServer.ExportPhotos
{
    [TestFixture()]
    public class ExportEmployeePhotos
    {
        private HrsContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new HrsContext();
        }

        [Test]
        public void ExtractPhotosToLocalDrive()
        {
            foreach (var emp in _context.Employee.ToList())
            {
                if (emp.Photo == null) continue;
                

                var bytes = emp.Photo;
                if (bytes.Length == 0) continue;

                string createdFileName = $"D:\\EmployeeImages\\{emp.PayrollID}.jpg";
                System.IO.File.WriteAllBytes(createdFileName, bytes);
            }
        }

        

    }
}
