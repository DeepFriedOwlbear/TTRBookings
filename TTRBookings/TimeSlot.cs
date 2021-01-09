using System;

namespace TTRBookings
{
    public class TimeSlot
    {
        public DateTime Start { get; private set; }

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
    }
}
