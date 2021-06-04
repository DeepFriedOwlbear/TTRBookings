using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TTRBookings.Core.Entities;

namespace TTRBookings.Infrastructure.Data
{
    public static class RepositorySeed
    {
        //static TTRBookingsContext context;
        public static void SeedDatabase(TTRBookingsContext context)
        {
            //using var context = new TTRBookingsContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            
            if (!context.Houses.Any())
            {
                House house = CreateHouse();
                House house2 = CreateHouse2();

                context.Houses.Add(house);
                context.Houses.Add(house2);

                context.SaveChanges();
            }
            context.SaveChanges();
        }     
        
        private static House CreateHouse()
        {
            //--input for calculations
            House house = new House("House_1");

            TierRate tierRate1 = house.CreateTierRate("1_Tier1", 50000);
            TierRate tierRate2 = house.CreateTierRate("1_Tier2", 100000);

            Rose rose1 = house.CreateRose("1_Rose1");
            Rose rose2 = house.CreateRose("1_Rose2");

            house.CreateManager("1_Alice");
            house.CreateManager("1_Bob");

            Room room1 = house.CreateRoom("1_Room1");
            Room room2 = house.CreateRoom("1_Room2");
            Room room3 = house.CreateRoom("1_Room3");
            Room room4 = house.CreateRoom("1_Room4");

            DateTime timeslot_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 00, 00, DateTimeKind.Utc);
            DateTime timeslot_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 00, 00, DateTimeKind.Utc);

            timeslot_start = timeslot_start.AddDays(1);
            timeslot_end = timeslot_end.AddDays(1);

            TimeSlot timeslot1 = new TimeSlot(timeslot_start, timeslot_end);
            TimeSlot timeslot2 = new TimeSlot(timeslot_start.AddHours(12), timeslot_end.AddHours(14));
            TimeSlot timeslot3 = new TimeSlot(timeslot_start.AddHours(15), timeslot_end.AddHours(18));
            TimeSlot timeslot4 = new TimeSlot(timeslot_start.AddHours(2), timeslot_end.AddHours(1));

            house.AddBooking(rose1, tierRate1, room1, timeslot1);
            house.AddBooking(rose2, tierRate2, room2, timeslot2);
            house.AddBooking(rose1, tierRate2, room1, timeslot3);
            house.AddBooking(rose2, tierRate1, room3, timeslot4);

            return house;
            //--end of input for calculation
        }

        private static House CreateHouse2()
        {
            //--input for calculations
            House house = new House("House_2");

            TierRate tierRate1 = house.CreateTierRate("2_Tier1", 20000);
            TierRate tierRate2 = house.CreateTierRate("2_Tier2", 50000);
            TierRate tierRate3 = house.CreateTierRate("2_Tier2", 75000);

            Rose rose1 = house.CreateRose("2_Rose1");
            Rose rose2 = house.CreateRose("2_Rose2");
            Rose rose3 = house.CreateRose("2_Rose3");

            house.CreateManager("2_Alice");
            house.CreateManager("2_Bob");

            Room room1 = house.CreateRoom("2_Room1");
            Room room2 = house.CreateRoom("2_Room2");
            Room room3 = house.CreateRoom("2_Room3");
            Room room4 = house.CreateRoom("2_Room4");

            DateTime timeslot_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00, DateTimeKind.Utc);
            DateTime timeslot_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 00, DateTimeKind.Utc);

            timeslot_start = timeslot_start.AddDays(1);
            timeslot_end = timeslot_end.AddDays(1);

            TimeSlot timeslot1 = new TimeSlot(timeslot_start, timeslot_end);
            TimeSlot timeslot2 = new TimeSlot(timeslot_start.AddHours(3), timeslot_end.AddHours(4));
            TimeSlot timeslot3 = new TimeSlot(timeslot_start.AddHours(12), timeslot_end.AddHours(15));
            TimeSlot timeslot4 = new TimeSlot(timeslot_start.AddHours(3), timeslot_end.AddHours(4));

            house.AddBooking(rose1, tierRate1, room1, timeslot1);
            house.AddBooking(rose2, tierRate2, room2, timeslot2);
            house.AddBooking(rose3, tierRate3, room3, timeslot3);
            house.AddBooking(rose2, tierRate1, room4, timeslot4);

            return house;
            //--end of input for calculation
        }
    }
}