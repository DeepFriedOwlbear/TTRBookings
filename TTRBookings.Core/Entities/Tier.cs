using System;


namespace TTRBookings.Core.Entities
{
    public class Tier : BaseEntity
    {
        public Guid RoseId { get; set; }
        public int Rate { get; set; }
        public int Unit { get; set; }
        public decimal Discount { get; set; } = 1;
        public decimal Revenue => Calculator.DoBaseCalculation(this);
    }
}
