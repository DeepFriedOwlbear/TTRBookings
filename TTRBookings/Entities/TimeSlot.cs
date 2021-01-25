using System;
using System.Collections.Generic;
using System.Diagnostics;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TimeSlot : BaseEntity
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public TimeSlot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        // Dictionaries compare keys based on equality,
        // this forces it to compare the actual value instead of the memory-reference.
        public override bool Equals(object obj)
        {
            if (obj is TimeSlot slot)
            {
                return Start == slot.Start && End == slot.End;
            }
            return false;            
        }

        public override int GetHashCode() => HashCode.Combine(Start, End);

        // These are just for convenience.
        public static bool operator ==(TimeSlot left, TimeSlot right) => Equals(left, right);
        public static bool operator !=(TimeSlot left, TimeSlot right) => !(left == right);

        private string GetDebuggerDisplay()
        {
            return $"{Start:yyyy-MM-dd HH:mm}";
        }
    }
}
