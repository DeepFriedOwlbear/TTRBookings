using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using TTRBookings.Data;
using TTRBookings.Entities;

namespace TTRBookings
{
    public class Program
    {
        //TODO - Check if Rose or Room is booked in AddBooking() before adding it to Bookings & RoomBookings.
        //TODO - DisplayBookings() should use the DB instead of the house object.

        static void Main(string[] args)
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

            //--end of input for calculation

            SeedDatabase(house);
            DisplayBookings(house);
        }

        private static void SeedDatabase(House house)
        {
            using var context = new TTRBookingsContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            if (!context.Houses.Any())
            {
                context.Houses.Add(house);
                context.SaveChanges();
            }
            context.SaveChanges();
        }

        private static void DisplayBookings(House house)
        {
            TimeSlot lastTimeslot = null;
            using var context = new TTRBookingsContext();

            foreach (Booking booking in house.Bookings.OrderBy(k => k.TimeSlot.Start))
            {
                if (lastTimeslot == null || lastTimeslot != booking.TimeSlot)
                {
                    Console.WriteLine($"{booking.TimeSlot.Start:yyyy-MM-dd HH:mm} - {booking.TimeSlot.End:HH:mm}");
                    Console.WriteLine($"{booking.Room.Name} > {booking.Rose.Name}");
                }
                else
                {
                    Console.WriteLine();
                }

                lastTimeslot = booking.TimeSlot;
                Console.WriteLine();
            }

            Console.WriteLine(JsonConvert.SerializeObject(house, Formatting.Indented));

            //Console.WriteLine(JsonSerializer.Serialize(house, new JsonSerializerOptions() { WriteIndented = true }));

            //Console.WriteLine($"TotalRevenue: {rose1.TotalRevenue()}");
            //Console.WriteLine($"ManagerCut: {house.ManagerCut}");
        }
    }
}
