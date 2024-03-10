using Microsoft.EntityFrameworkCore;
using PishgamanTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishgamanTask.Infrastructure.Database
{
    public partial class PishgamanContext : DbContext
    {
        public PishgamanContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public PishgamanContext(DbContextOptions<PishgamanContext> options) : base (options)
        {
            
        }

        public virtual DbSet<Person> Tbl_People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
