using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Core
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
