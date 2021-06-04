using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace TTRBookings.Core.Entities
{
    public class Booking : BaseEntity
    {
        public Guid HouseId { get; private set; }
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

        public static Booking Create(Guid houseId, Rose rose, Tier tier, Room room, TimeSlot timeslot)
        {
            var booking = new Booking(rose, tier, room, timeslot);
            booking.HouseId = houseId;
            return booking;
        }

        public void Update(Rose rose, Room room, Tier tier, TimeSlot timeSlot)
        {
            Rose = rose ?? throw new ArgumentNullException(nameof(rose));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot));
        }

        public void Update(Rose rose) { Rose = rose ?? throw new ArgumentNullException(nameof(rose)); }
        public void Update(Room room) { Room = room ?? throw new ArgumentNullException(nameof(room)); }
        public void Update(Tier tier) { Tier = tier ?? throw new ArgumentNullException(nameof(tier)); }
        public void Update(TimeSlot timeSlot) { TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot)); }
    }
}