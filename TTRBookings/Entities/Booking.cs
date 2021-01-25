using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class Booking : BaseEntity
    {
        public Rose Rose { get; private set; }
        public Room Room { get; private set; }
        public Tier Tier { get; private set; }
        public TimeSlot TimeSlot { get; private set; }

        private Booking() { } //needed by EntityFramework

        private Booking(Rose rose, Tier tier, Room room, TimeSlot timeSlot)
        {
            Rose = rose ?? throw new ArgumentNullException(nameof(rose));
            Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot));
        }

        public static Booking Create(Rose rose, Tier tier, Room room, TimeSlot timeslot)
        {
            return new Booking(rose, tier, room, timeslot);
        }


        //TODO: THINGS BELOW DON'T NEED TO BE IN BOOKINGS.
        public static IList<Booking> Load<TBaseEntity>(TBaseEntity baseEntity)
        {
            return baseEntity switch
            {
                Rose rose => Load(b => b.Rose == rose),
                Room room => Load(b => b.Room == room),
                Tier tier => Load(b => b.Tier == tier),
                TimeSlot timeslot => Load(b => b.TimeSlot == timeslot),
                _ => Array.Empty<Booking>(),
                //_ => throw new ArgumentException($"Argument Passed was not a supported type, {typeof(TBaseEntity).Name}")
            };
        }

        private static IList<Booking> Load(Expression<Func<Booking, bool>> predicate)
        {
            //load context
            //find bookings in context where booking matches predicate
            //return bookings
            using var context = new TTRBookingsContext();

            var bookings = context.Set<Booking>()
                .Include(r => r.Rose)
                    .ThenInclude(rt => rt.Tiers)
                .Include(r => r.Tier)
                .Include(r => r.Room)
                .Include(r => r.TimeSlot)
                .Where(predicate)
                .ToList();

            return bookings;
        }
    }
}