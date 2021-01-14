using System;

namespace TTRBookings.Entities
{
    public class TierRate
    {
        public Guid Id { get; set; }
        public string Tier { get; set; }
        public int Value { get; set; }
        public TierRate(string tier, int value)
        {
            Tier = tier;
            Value = value;
        }
    }
}