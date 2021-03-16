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
            House house = new House();

            TierRate tierRate1 = house.CreateTierRate("1_Tier1", 50000);
            TierRate tierRate2 = house.CreateTierRate("1_Tier2", 100000);

            Rose rose1 = house.CreateRose("1_Rose1");
            Rose rose2 = house.CreateRose("1_Rose2");

            house.CreateManager("1_Alice");
            house.CreateManager("1_Bob");

            Room room1 = house.CreateRoom("Room1");
            Room room2 = house.CreateRoom("Room2");
            Room room3 = house.CreateRoom("Room3");
            Room room4 = house.CreateRoom("Room4");

            house.AddBooking(rose1, tierRate1, room1, new TimeSlot(new DateTime(2021, 1, 4, 20, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 4, 22, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose2, tierRate2, room2, new TimeSlot(new DateTime(2021, 1, 4, 19, 30, 00, DateTimeKind.Utc), new DateTime(2021, 1, 4, 23, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose1, tierRate2, room1, new TimeSlot(new DateTime(2021, 1, 4, 22, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 5, 01, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose2, tierRate1, room3, new TimeSlot(new DateTime(2021, 1, 4, 23, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 5, 00, 30, 00, DateTimeKind.Utc)));

            return house;
            //--end of input for calculation
        }

        private static House CreateHouse2()
        {
            //--input for calculations
            House house = new House();

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

            house.AddBooking(rose1, tierRate1, room1, new TimeSlot(new DateTime(2021, 1, 4, 14, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 4, 16, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose2, tierRate2, room2, new TimeSlot(new DateTime(2021, 1, 4, 15, 30, 00, DateTimeKind.Utc), new DateTime(2021, 1, 4, 18, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose3, tierRate3, room3, new TimeSlot(new DateTime(2021, 1, 4, 16, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 5, 17, 00, 00, DateTimeKind.Utc)));
            house.AddBooking(rose2, tierRate1, room4, new TimeSlot(new DateTime(2021, 1, 4, 19, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 5, 01, 30, 00, DateTimeKind.Utc)));

            return house;
            //--end of input for calculation
        }
    }
}