using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class TierVM
{
    public Guid Id { get; set; }
    public decimal Rate { get; set; }
    public int Unit { get; set; }
    public decimal Discount { get; set; }
    public decimal Revenue { get; set; }

    public static TierVM Create(Tier tier)
    {
        return new TierVM()
        {
            Id = tier.Id,
            Rate = tier.Rate,
            Unit = tier.Unit,
            Discount = tier.Discount,
            Revenue = tier.Revenue
        };
    }
}