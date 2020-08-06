using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using slushiecorp.Models;

namespace slushiecorp.Data
{
    public class slushiecorpContext : DbContext
    {
        public slushiecorpContext (DbContextOptions<slushiecorpContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(c => c.Customer)
                .WithMany(o => o.Orders);

        }

        public DbSet<slushiecorp.Models.Customer> Customer { get; set; }

        public DbSet<slushiecorp.Models.Order> Order { get; set; }

        private bool IsNewCustomer(int CustomerId)
        {
            return this.Customer.Any(c => c.CustomerID == CustomerId);
        }
    }
}
