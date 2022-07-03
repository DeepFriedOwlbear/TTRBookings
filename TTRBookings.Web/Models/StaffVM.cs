using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class StaffVM
{
    public Guid Id { get; set; }
    public Guid HouseId { get; set; }
    public string Name { get; set; }
    public decimal TotalRevenue { get; set; }
    public IList<Tier> Tiers { get; set; } = new List<Tier>();

    public static StaffVM Create(Staff staff)
    {
        return new StaffVM()
        {
            Id = staff.Id,
            HouseId = staff.HouseId,
            Name = staff.Name,
            Tiers = staff.Tiers,
            TotalRevenue = staff.TotalRevenue()
        };
    }
}