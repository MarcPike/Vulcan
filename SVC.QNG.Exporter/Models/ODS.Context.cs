﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SVC.QNG.Exporter.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ODSContext : DbContext
    {
        public ODSContext()
            : base("name=ODSContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Vulcan_CrmQuoteItem> Vulcan_CrmQuoteItem { get; set; }
        public virtual DbSet<Vulcan_CrmQuoteItem_ProductionCost> Vulcan_CrmQuoteItem_ProductionCost { get; set; }
        public virtual DbSet<Vulcan_CrmQuoteItem_TestPieces> Vulcan_CrmQuoteItem_TestPieces { get; set; }
        public virtual DbSet<Vulcan_CrmQuote> Vulcan_CrmQuote { get; set; }
    }
}