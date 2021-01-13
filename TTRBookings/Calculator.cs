using System;
using System.Collections.Generic;
using TTRBookings.Entities;

namespace TTRBookings
{
    public class Calculator
    {
        public readonly static TimeSpan TimeUnit = TimeSpan.FromMinutes(30);

        public static decimal DoBaseCalculation(Tier tier)
        {
            return tier.Rate * tier.Unit * tier.Discount;
        }

        public static decimal DoManagerCalculation(int managerCount, decimal revenue)
        {
            return revenue / managerCount;
        }
    }
}
