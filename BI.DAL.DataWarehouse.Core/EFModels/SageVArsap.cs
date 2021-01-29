using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVArsap
    {
        public string Coid { get; set; }
        public string Codeslsp { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public short Swactv { get; set; }
        public decimal Dateinac { get; set; }
        public decimal Datelastmn { get; set; }
        public string Codeempl { get; set; }
        public string Nameempl { get; set; }
        public short Swcomm { get; set; }
        public decimal Amtanltarg { get; set; }
        public decimal Salesbase1 { get; set; }
        public decimal Salesbase2 { get; set; }
        public decimal Salesbase3 { get; set; }
        public decimal Salesbase4 { get; set; }
        public decimal Salesrate1 { get; set; }
        public decimal Salesrate2 { get; set; }
        public decimal Salesrate3 { get; set; }
        public decimal Salesrate4 { get; set; }
        public decimal Salesrate5 { get; set; }
        public decimal Salescomm { get; set; }
        public decimal Salescost { get; set; }
        public decimal Dateclrd { get; set; }
    }
}
