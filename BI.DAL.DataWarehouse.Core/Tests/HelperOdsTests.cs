using System;
using System.Collections.Generic;
using System.Diagnostics;
using BI.DAL.DataWarehouse.Core.Helpers;
using NUnit.Framework;

namespace BI.DAL.DataWarehouse.Core.Tests
{

    [TestFixture]
    public class HelperOdsTests
    {
        private HelperOds _helperOds = new HelperOds();

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void GetWarehouses()
        {
            var coidList = new List<string>() { "INC", "CAN"};

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var cacheInfo = _helperOds.RefreshWarehouseCache();
            sw.Stop();
            Console.WriteLine(ObjectDumper.Dump(cacheInfo));
            Console.WriteLine();

            sw.Start();
            var warehouses = _helperOds.GetWarehousesForCoids(coidList, false);
            sw.Stop();
            Console.WriteLine($"Time to get (INC,CAN) Warehouses: {sw.Elapsed} Count: {warehouses.Count}");
            Console.WriteLine();

            //foreach (var warehouse in warehouses)
            //{
            //    Console.WriteLine($"{warehouse.Coid} - {warehouse.Warehouse} Status: {warehouse.Status}");
            //}

            sw.Start();
            coidList = new List<string>() {"EUR"};
            warehouses = _helperOds.GetWarehousesForCoids(coidList, false);
            sw.Stop();
            Console.WriteLine($"Time to get (EUR) Warehouses: {sw.Elapsed} Count: {warehouses.Count}");
            Console.WriteLine();

            //foreach (var warehouse in warehouses)
            //{
            //    Console.WriteLine($"{warehouse.Coid} - {warehouse.Warehouse} Status: {warehouse.Status}");
            //}

        }

        [Test]
        public void GetBranches()
        {
            var coidList = _helperOds.GetCoidList();
            foreach (var coid in coidList)
            {
                var branches = _helperOds.GetBranchList(new List<string>() {coid});
                Console.WriteLine($"{coid} Branches:");
                foreach (var branch in branches)
                {
                    Console.WriteLine($"  Code: {branch.Code} Name: {branch.Name} Status: {branch.Status} Type: {branch.Type}");
                }
            }
        }

        [Test]
        public void GetBusinessUnitWarehouses()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var coidList = _helperOds.GetCoidList();
            foreach (var coid in coidList)
            {
                var businessUnitWarehouses = _helperOds.GetBusinessUnitWarehouses(new List<string>() { coid });
                Console.WriteLine($"{coid} Coid:");
                foreach (var businessUnitWarehouse in businessUnitWarehouses)
                {
                    Console.WriteLine($"BU#: {businessUnitWarehouse.BusinessUnitId}  BusinessUnit: {businessUnitWarehouse.BusinessUnit} Warehouse: {businessUnitWarehouse.Warehouse}");
                }
            }
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
        }

    }
}
