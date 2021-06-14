using System;
using System.Diagnostics;

namespace TTRBookings.Core.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
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

        private string GetDebuggerDisplay()
        {
            return Rate.ToString();
        }
    }
}
