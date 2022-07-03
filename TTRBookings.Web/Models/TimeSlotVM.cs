using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class TimeSlotVM
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public static TimeSlotVM Create(TimeSlot timeslot)
    {
        return new TimeSlotVM
        {
            Id = timeslot.Id,
            Start = timeslot.Start,
            End = timeslot.End
        };
    }

    public override string ToString()
    {
        return $"{Start:yyyy-MM-dd HH:mm}";
    }
}