namespace Warehouse_23DH113071.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Warehouse_23DH113071Entities : DbContext
    {
        public Warehouse_23DH113071Entities()
            : base("name=Warehouse_23DH113071Entities")
        {
            Database.SetInitializer<Warehouse_23DH113071Entities>(null);
        }

        public Warehouse_23DH113071Entities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<Warehouse_23DH113071Entities>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}
