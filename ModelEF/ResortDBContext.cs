using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class ResortDBContext:DbContext
    {
        public ResortDBContext(DbContextOptions<ResortDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingServices>()
                .HasKey(p => new { p.BookingID, p.ServiceID });
            modelBuilder.Entity<SuppliesForRoom>()
                .HasKey(p => new { p.RoomID, p.SupplyID });
        }

        #region Models DbSet
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingServices> BookingServices { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SuppliesForRoom> SuppliesForRooms { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Rule> Rules { get; set; }
        #endregion
    }
}
