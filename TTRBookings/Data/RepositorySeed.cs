using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TTRBookings.Entities;

namespace TTRBookings.Data
{
    public static class RepositorySeed
    {
        public static void SeedDatabase()
        {
            using var context = new TTRBookingsContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            if (!context.Houses.Any())
            {
                House house = CreateHouse();
                context.Houses.Add(house);
                context.SaveChanges();
            }
            context.SaveChanges();
        }

        private static House CreateHouse()
        {
            //--input for calculations
            House house = new House();

            TierRate tierRate1 = house.CreateTierRate("Tier1", 50000);
            TierRate tierRate2 = house.CreateTierRate("Tier2", 100000);

            Rose rose1 = house.CreateRose("Rose1");
            Rose rose2 = house.CreateRose("Rose2");

            house.CreateManager("Alice");
            house.CreateManager("Bob");

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
    }
}