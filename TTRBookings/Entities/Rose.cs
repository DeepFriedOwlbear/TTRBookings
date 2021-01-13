using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRBookings.Entities
{
    public class Rose
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Tier> Tiers { get; set; } = new List<Tier>();

        public Rose(string name)
        {
            Name = name;
        }

        public void AddTier(TierRate rate) => AddTier(rate, 1);
        public void AddTier(TierRate rate, int amount)
        {            
            //check if tier is in the list, if so add an increment, else add it to the list
            Tier present = Tiers.FirstOrDefault(t => t.Rate == rate.Value);

            if (present != null)
            {
                present.Unit += amount;
            }
            else
            {
                Tier tier = new Tier() { Rate = rate.Value, Unit = amount };
                Tiers.Add(tier);
            }
        }

        public decimal TotalRevenue()
        {
            return Tiers.Sum(t => t.Revenue);
        }
    }
}
