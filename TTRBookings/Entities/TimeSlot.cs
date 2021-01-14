using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TTRBookings.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public DateTime Start { get; private set; }
        public IList<Room> Rooms { get; set; } = new List<Room>();

        public TimeSlot(DateTime start)
        {
            Start = start;
        }

        // Dictionaries compare keys based on equality,
        // this forces it to compare the actual value instead of the memory-reference.
        public override bool Equals(object obj) => obj is TimeSlot slot && Start == slot.Start;
        public override int GetHashCode() => HashCode.Combine(Start);

        // These are just for convenience.
        public static bool operator ==(TimeSlot left, TimeSlot right) => Equals(left, right);
        public static bool operator !=(TimeSlot left, TimeSlot right) => !(left == right);

        private string GetDebuggerDisplay()
        {
            return $"{Start:yyyy-MM-dd HH:mm}";
        }
    }
}
