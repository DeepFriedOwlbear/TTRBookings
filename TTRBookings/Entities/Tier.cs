﻿using System;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class Tier : BaseEntity
    {
        public int Rate { get; set; }
        public int Unit { get; set; }
        public decimal Discount { get; set; } = 1;
        public decimal Revenue => Calculator.DoBaseCalculation(this);
    }
}
