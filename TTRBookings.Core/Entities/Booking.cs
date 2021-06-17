using System;
using System.Diagnostics;


namespace TTRBookings.Core.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Booking : BaseEntity
    {
        public Guid HouseId { get; private set; }
        public Staff Staff { get; private set; }
        public Room Room { get; private set; }
        public Tier Tier { get; private set; }
        public TimeSlot TimeSlot { get; private set; }

        private Booking() { } //needed by EntityFramework

        private Booking(Staff staff, Tier tier, Room room, TimeSlot timeSlot)
        {
            Staff = staff ?? throw new ArgumentNullException(nameof(staff));
            Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot));
        }

        public static Booking Create(Guid houseId, Staff staff, Tier tier, Room room, TimeSlot timeslot)
        {
            var booking = new Booking(staff, tier, room, timeslot);
            booking.HouseId = houseId;
            return booking;
        }

        private Booking Update(Staff staff, Room room, Tier tier, TimeSlot timeSlot)
        {
            Staff = staff ?? throw new ArgumentNullException(nameof(staff));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot));

            return this;
        }

        public Booking Update(Staff staff) { return Update(staff, Room, Tier, TimeSlot); }
        public Booking Update(Room room) { return Update(Staff, room, Tier, TimeSlot); }
        public Booking Update(Tier tier) { return Update(Staff, Room, tier, TimeSlot); }
        public Booking Update(TimeSlot timeSlot) { return Update(Staff, Room, Tier, timeSlot); }

        private string GetDebuggerDisplay()
        {
            return Staff.Name + "; " + Room.Name + "; " + Tier.Rate.ToString();
        }
    }
}