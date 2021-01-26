using Microsoft.EntityFrameworkCore;
using TTRBookings.Core.Entities;

namespace TTRBookings.Infrastructure.Data
{
    //add-migration Initial -p TTRBookings.Infrastructure -s TTRBookings.Web -o Data/Migrations
    public class TTRBookingsContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Rose> Roses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<TierRate> TierRates { get; set; }
        public DbSet<Tier> Tiers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public TTRBookingsContext(DbContextOptions<TTRBookingsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<House>()
                .HasMany(o => o.Roses)
                .WithOne()
                .HasForeignKey(o => o.HouseId);
        }

        //public override int SaveChanges()
        //{
        //    //foreach all the changed entities.
        //    //check all entities marked for deletion, and set isdeleted flag.
        //    //change to modified instead of delete flag.

        //    //foreach(var entry in this.ChangeTracker.Entries())
        //    //{
        //    //    if(entry.State == EntityState.Deleted)
        //    //    {
        //    //        //set isdeletedflag
        //    //        //entry.CurrentValues.
        //    //        //entry.State = EntityState.Modified;
        //    //    }
        //    //}

        //    return base.SaveChanges();
        //}
    }
}
