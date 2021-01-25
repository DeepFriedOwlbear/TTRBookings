using System;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class TierRate : BaseEntity
    {
        public string Tier { get; set; }
        public int Value { get; set; }
        public TierRate(string tier, int value)
        {
            Tier = tier;
            Value = value;
        }
    }
}