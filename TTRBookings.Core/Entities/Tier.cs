using System;


namespace TTRBookings.Core.Entities
{
    public class Tier : BaseEntity
    {
        public Guid HouseId { get; set; }
        public int Rate { get; set; }
        public int Unit { get; set; }

        public Tier(int rate)
        {
            Rate = rate;
        }

        public decimal Discount { get; set; } = 1;
        public decimal Revenue => Calculator.DoBaseCalculation(this);
    }
}
