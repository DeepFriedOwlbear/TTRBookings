using System;
using System.Diagnostics;


namespace TTRBookings.Core.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TimeSlot : BaseEntity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeSlot(DateTime start, DateTime end)
        {
            if(start > end) throw new BookingStartTimeAfterEndTime(nameof(start));
            if(start < DateTime.Now) throw new BookingStartTimeInThePast(nameof(start));
            if((end - start).TotalHours >= 24) throw new BookingDurationLongerThan24Hours(nameof(end));
            
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
            return $"{Start:dd-MM-yyyy HH:mm} - {End:dd-MM-yyyy HH:mm}";
        }
    }

    public class BookingStartTimeAfterEndTime : Exception
    {
        public BookingStartTimeAfterEndTime(){}

        public BookingStartTimeAfterEndTime(string message)
            : base(message){}
    }

    public class BookingStartTimeInThePast : Exception
    {
        public BookingStartTimeInThePast(){}

        public BookingStartTimeInThePast(string message)
            : base(message){}
    }

    public class BookingDurationLongerThan24Hours : Exception
    {
        public BookingDurationLongerThan24Hours(){}

        public BookingDurationLongerThan24Hours(string message)
            : base(message){}
    }
}
