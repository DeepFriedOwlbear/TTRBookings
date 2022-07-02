using System;

namespace TTRBookings.Web.DTOs;

public sealed class BookingDTO
{
    public Guid BookingId { get; set; }
    public Guid StaffId { get; set; }
    public Guid RoomId { get; set; }
    public string TierRate { get; set; }
    public string TimeStart { get; set; }
    public string TimeEnd { get; set; }
}
