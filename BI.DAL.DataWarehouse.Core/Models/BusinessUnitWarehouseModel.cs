using System;
using System.Collections.Generic;
using System.Text;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public class BusinessUnitWarehouseModel
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnit { get; set; }
        public string Warehouse { get; set; }
        public string Coid { get; set; }

        public BusinessUnitWarehouseModel()
        {
            
        }

        public BusinessUnitWarehouseModel(string coid, string warehouse, int businessUnitId)
        {
            Coid = coid;
            Warehouse = warehouse;
            BusinessUnitId = businessUnitId;
        }

    }
}
