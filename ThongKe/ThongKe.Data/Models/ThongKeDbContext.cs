namespace ThongKe.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ThongKeDbContext : DbContext
    {
        public ThongKeDbContext()
            : base("name=ThongKeDbContext")
        {
        }

        public virtual DbSet<account> accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .Property(e => e.chinhanh)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.khoi)
                .IsUnicode(false);
        }
    }
}
