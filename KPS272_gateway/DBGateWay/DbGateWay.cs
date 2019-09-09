namespace KPS272_gateway.DBGateWay
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbGateWay : DbContext
    {
        public DbGateWay()
            : base("name=DbGateWay")
        {
        }

        public virtual DbSet<vfd_node_registry> vfd_node_registry { get; set; }
        public virtual DbSet<vfd_node00001> vfd_node00001 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
