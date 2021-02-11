using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using TTRBookings.Core;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models
{
    public class BookingVM
    {
        public Guid Id { get; set; }
        public RoseVM Rose { get; set; }
        public RoomVM Room { get; set; }
        public TierVM Tier { get; set; }
        public TimeSlotVM TimeSlot { get; set; }

        public static BookingVM Create(Booking booking)
        {
            return new BookingVM()
            {
                Id = booking.Id,
                Rose = RoseVM.Create(booking.Rose),
                Room = RoomVM.Create(booking.Room),
                Tier = TierVM.Create(booking.Tier),
                TimeSlot = TimeSlotVM.Create(booking.TimeSlot)
            };
        }
    }

    public class RoseVM
    {
        public Guid Id { get; set; }
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public decimal TotalRevenue { get; set; }
        //public IList<Tier> Tiers { get; set; } = new List<Tier>();

        public static RoseVM Create(Rose rose)
        {
            return new RoseVM()
            {
                Id = rose.Id,
                HouseId = rose.HouseId,
                Name = rose.Name,
                TotalRevenue = rose.TotalRevenue()
            };
        }
    }
    public class RoomVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public static RoomVM Create(Room room)
        {
            return new RoomVM()
            {
                Id = room.Id,
                Name = room.Name
            };
        }
    }
    public class TierVM
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public int Unit { get; set; }
        public decimal Discount { get; set; }
        public decimal Revenue { get; set; }

        public static TierVM Create(Tier tier)
        {
            return new TierVM()
            {
                Id = tier.Id,
                Rate = tier.Rate,
                //Unit = tier.Unit,
                //Discount = tier.Discount,
                //Revenue = tier.Revenue
            };
        }
    }
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
}