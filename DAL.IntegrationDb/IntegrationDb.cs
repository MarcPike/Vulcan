namespace DAL.IntegrationDb
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IntegrationDb : DbContext
    {
        public IntegrationDb()
            : base("name=IntegrationDb")
        {
        }

        public IntegrationDb(string connectionString) : base(connectionString)
        {

        }

        public static IntegrationDb UsingEmbeddedConnectionString()
        {
            return new IntegrationDb(ConnectionStringHelper.GetConnectionString());
        }


        public virtual DbSet<import_part_specifications> import_part_specifications { get; set; }
        public virtual DbSet<import_sales_charges> import_sales_charges { get; set; }
        public virtual DbSet<import_sales_costs> import_sales_costs { get; set; }
        public virtual DbSet<import_sales_headers> import_sales_headers { get; set; }
        public virtual DbSet<import_sales_items> import_sales_items { get; set; }
        public virtual DbSet<import_stock_allocations> import_stock_allocations { get; set; }
        public virtual DbSet<MessageLog> MessageLogs { get; set; }
        public virtual DbSet<XRefCustomerId> XRefCustomerIds { get; set; }
        public virtual DbSet<XRefUnitOfMeasure> XRefUnitOfMeasures { get; set; }
        public virtual DbSet<IMetalStagingTest> IMetalStagingTests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim1)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim1_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim1_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim2)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim2_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim2_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim3)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim3_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim3_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim4)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim4_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim4_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim5)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim5_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.dim5_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.entry_quantity)
                .HasPrecision(12, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.standard_quantity)
                .HasPrecision(12, 3);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.standard_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim1)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim1_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim1_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim2)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim2_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim2_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim3)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim3_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim3_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim4)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim4_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim4_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim5)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim5_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.consumed_dim5_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.outside_diameter)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.outside_diameter_minimum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.outside_diameter_maximum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.inside_diameter)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.inside_diameter_minimum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.inside_diameter_maximum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_height)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_height_minimum)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_height_maximum)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_weight_minimum)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.pack_weight_maximum)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.adjustment_price)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_part_specifications>()
                .Property(e => e.yield_percentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<import_sales_charges>()
                .Property(e => e.import_source)
                .IsUnicode(false);

            modelBuilder.Entity<import_sales_charges>()
                .Property(e => e.charge)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_charges>()
                .Property(e => e.exchange_rate)
                .HasPrecision(14, 8);

            modelBuilder.Entity<import_sales_charges>()
                .Property(e => e.charge_fix_status)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_charges>()
                .Property(e => e.charge_visibility)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_costs>()
                .Property(e => e.import_source)
                .IsUnicode(false);

            modelBuilder.Entity<import_sales_costs>()
                .Property(e => e.cost)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_costs>()
                .Property(e => e.exchange_rate)
                .HasPrecision(14, 8);

            modelBuilder.Entity<import_sales_costs>()
                .Property(e => e.cost_fix_status)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_costs>()
                .Property(e => e.visibility)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.import_status)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.import_action)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_cost_rate)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_cost_amount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_exchange_rate)
                .HasPrecision(14, 8);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_exchange_rate_type_code)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.exchange_rate)
                .HasPrecision(14, 8);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.exchange_rate_type_code)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.settlement_discount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_charge_rate)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transport_charge_amount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<import_sales_headers>()
                .Property(e => e.transfer_type)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.import_source)
                .IsUnicode(false);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim1)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim1_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim1_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim2)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim2_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim2_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim3)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim3_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim3_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim4)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim4_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim4_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim5)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim5_negative_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.dim5_positive_tolerance)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.required_quantity)
                .HasPrecision(12, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.required_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.original_quantity)
                .HasPrecision(12, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.margin_type)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.transfer_type)
                .IsFixedLength();

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.outside_diameter)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.outside_diameter_minimum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.outside_diameter_maximum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_height)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_height_minimum)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_height_maximum)
                .HasPrecision(9, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_weight_minimum)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.pack_weight_maximum)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.inside_diameter)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.inside_diameter_minimum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.inside_diameter_maximum)
                .HasPrecision(9, 2);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.invoice_costing_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.mixture_price)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.fabrication_price)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.adjustment_price)
                .HasPrecision(12, 4);

            modelBuilder.Entity<import_sales_items>()
                .Property(e => e.yield_percentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_company_reference)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_source)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.stock_branch_code)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.stock_item_number)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.composite_key)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_notes)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_user_name)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.import_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.allocation_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.allocation_branch_code)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.allocated_quantity)
                .HasPrecision(12, 3);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.allocated_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.packing_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.invoice_weight)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_stock_allocations>()
                .Property(e => e.automated_process_type)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
