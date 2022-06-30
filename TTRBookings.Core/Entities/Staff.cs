using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace TTRBookings.Core.Entities;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Staff : BaseEntity
{
    public string Name { get; set; }
    public Guid HouseId { get; set; }
    public IList<Tier> Tiers { get; set; } = new List<Tier>();
    public Staff(string name)
    {
        Name = name;
    }

    public void AddTier(Tier tier)
    {
        Tier currentTier = Tiers.FirstOrDefault(t => t.Rate == tier.Rate);

        if (currentTier != null)
        {
            currentTier.Unit += tier.Unit;
        }
        else
        {
            Tiers.Add(tier);
        }
    }

    public void RemoveTier(Tier tier)
    {
        Tier currentTier = Tiers.FirstOrDefault(t => t.Rate == tier.Rate);

        if (currentTier != null)
        {
            if (currentTier.Unit > tier.Unit)
            {
                currentTier.Unit -= tier.Unit;
            }
            else if (currentTier.Unit == tier.Unit)
            {
                Tiers.Remove(currentTier);
            }
        }
    }

    public decimal TotalRevenue()
    {
        return Math.Round(Tiers.Sum(t => t.Revenue), 2);
    }

    private string GetDebuggerDisplay()
    {
        return Name;
    }
}
