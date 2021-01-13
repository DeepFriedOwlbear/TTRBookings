using System;
using System.Linq;
using TTRBookings;
using TTRBookings.Entities;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            House house = new House();

            Rose rose1 = house.CreateRose("TestRose1");
            Rose rose2 = house.CreateRose("TestRose2");

            Manager manager1 = house.CreateManager("Alice");
            Manager manager2 = house.CreateManager("Bob");

            //add tiers to house later
            int tier1 = 50000;
            int tier2 = 100000;
            int tier3 = 150000;

            rose1.AddTier(tier1, 2);
            rose1.AddTier(tier2, 3);

            rose2.AddTier(tier1, 1);
            rose2.AddTier(tier3, 4);


            DateTime dtime1 = new DateTime(2021, 1, 4, 20, 00, 00, DateTimeKind.Utc);
            DateTime dtime2 = new DateTime(2021, 1, 4, 19, 30, 00, DateTimeKind.Utc);
            TimeSlot timeslot1 = new TimeSlot(dtime1);
            TimeSlot timeslot2 = new TimeSlot(dtime2);

            //house.AddBooking(rose1, TierLevel.Tier1, timeslot1, 1);
            //house.AddBooking(rose2, TierLevel.Tier2, timeslot2, 3);

            Assert.Equal(3, house.Bookings.Count);
        }

        [Fact]
        public void CreateManager_ShouldAddToManagers_GivenCreateManagerCalled()
        {
            House house = new House();
            Manager manager1 = house.CreateManager("Alice");

            Assert.Equal(manager1, house.Managers.First());
        }
    }
}
