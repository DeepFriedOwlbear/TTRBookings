using System;


namespace TTRBookings.Core.Entities
{
    public class TierRate : BaseEntity
    {
        public Guid HouseId { get; set; }
        public string Tier { get; set; }
        public int Value { get; set; }
        public TierRate(string tier, int value)
        {
            Tier = tier;
            Value = value;
        }
    }
}