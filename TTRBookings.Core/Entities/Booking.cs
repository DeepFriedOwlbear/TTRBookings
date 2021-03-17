using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace TTRBookings.Core.Entities
{
    public class Booking : BaseEntity
    {
        public Guid HouseId { get; set; }
        public Rose Rose { get; set; }
        public Room Room { get; set; }
        public Tier Tier { get; set; }
        public TimeSlot TimeSlot { get; set; }

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
    }
}