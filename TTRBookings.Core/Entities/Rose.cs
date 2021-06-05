using System;
using System.Collections.Generic;
using System.Linq;


namespace TTRBookings.Core.Entities
{
    public class Rose : BaseEntity
    {
        public string Name { get; set; }
        public Guid HouseId { get; set; }
        public IList<Tier> Tiers { get; set; } = new List<Tier>();
        public Rose(string name)
        {
            Name = name;
        }

        public void AddTier(Tier tier)
        {
            Tier currentTier = Tiers.FirstOrDefault(t => t.Rate == tier.Rate);

            if (currentTier != null) currentTier.Unit += tier.Unit;
            else Tiers.Add(tier);
        }

        public void RemoveTier(Tier tier)
        {
            Tier currentTier = Tiers.FirstOrDefault(t => t.Rate == tier.Rate);

            if(currentTier != null)
            {
                if (currentTier.Unit > tier.Unit) currentTier.Unit -= tier.Unit;
                else if (currentTier.Unit == tier.Unit) Tiers.Remove(currentTier);
            }
        }

        public decimal TotalRevenue()
        {
            return Tiers.Sum(t => t.Revenue);
        }
    }
}
