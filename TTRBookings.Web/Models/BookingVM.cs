using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class BookingVM
{
    public Guid Id { get; set; }
    public StaffVM Staff { get; set; }
    public RoomVM Room { get; set; }
    public TierVM Tier { get; set; }
    public TimeSlotVM TimeSlot { get; set; }

    public static BookingVM Create(Booking booking)
    {
        return new BookingVM()
        {
            Id = booking.Id,
            Staff = StaffVM.Create(booking.Staff),
            Room = RoomVM.Create(booking.Room),
            Tier = TierVM.Create(booking.Tier),
            TimeSlot = TimeSlotVM.Create(booking.TimeSlot)
        };
    }
}