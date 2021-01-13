using System;

namespace TTRBookings.Entities
{
    public class Tier
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public int Unit { get; set; }
        public decimal Discount { get; set; } = 1;
        public decimal Revenue => Calculator.DoBaseCalculation(this);
    }
}
