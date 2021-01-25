using System;
using System.Collections.Generic;
using System.Linq;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class Rose : BaseEntity
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public IList<Tier> Tiers { get; set; } = new List<Tier>();
        public Rose(string name)
        {
            Name = name;
        }

        public void AddTier(Tier tier)
        {
            Tier present = Tiers.FirstOrDefault(t => t.Rate == tier.Rate);

            if (present != null)
            {
                present.Unit += tier.Unit;
            }
            else
            {
                Tiers.Add(tier);
            }
        }

        public decimal TotalRevenue()
        {
            return Tiers.Sum(t => t.Revenue);
        }
    }
}
