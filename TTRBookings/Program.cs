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
        //TODO - Check why TierRate and TimeSlot aren't saved to the DB.
        //TODO - DisplayBookings() should use the DB instead of the house object.

        static void Main(string[] args)
        {
            //--input for calculations
            House house = new House();

            TierRate tier1 = new TierRate();
            TierRate tier2 = new TierRate();
            tier1.Tier = "Tier1";
            tier2.Tier = "Tier2";
            tier1.Value = 50000;
            tier2.Value = 100000;

            Rose rose1 = house.CreateRose("Rose1");
            Rose rose2 = house.CreateRose("Rose2");

            house.CreateManager("Alice");
            house.CreateManager("Bob");

            Room room1 = house.CreateRoom("Room1");
            Room room2 = house.CreateRoom("Room2");
            Room room3 = house.CreateRoom("Room3");
            Room room4 = house.CreateRoom("Room4");

            house.AddBooking(rose1, tier1, room1, new TimeSlot(new DateTime(2021, 1, 4, 20, 00, 00, DateTimeKind.Utc)), 4);
            house.AddBooking(rose2, tier2, room2, new TimeSlot(new DateTime(2021, 1, 4, 20, 30, 00, DateTimeKind.Utc)), 6);
            house.AddBooking(rose1, tier1, room3, new TimeSlot(new DateTime(2021, 1, 4, 21, 00, 00, DateTimeKind.Utc)), 6);
            house.AddBooking(rose2, tier2, room4, new TimeSlot(new DateTime(2021, 1, 4, 22, 30, 00, DateTimeKind.Utc)), 4);
            //--end of input for calculation

            SeedDatabase(house);
            DisplayBookings(house);
        }

        private static void SeedDatabase(House house)
        {
            using var context = new TTRBookingsContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
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

            foreach (TimeSlot timeslot in house.Bookings.Keys.OrderBy(k => k.Start))
            {
                if (lastTimeslot == null || lastTimeslot != timeslot) 
                {
                    Console.WriteLine($"{timeslot.Start:yyyy-MM-dd HH:mm}");
                }
                else
                {
                    Console.WriteLine();
                }

                var existingRoomSlot = house.Bookings.FirstOrDefault(b => b.Key.Start == timeslot.Start);

                foreach (Room item in existingRoomSlot.Value)
                {
                    Console.WriteLine($"{item.Name}\t{item.Rose.Name}");
                }

                lastTimeslot = timeslot;
                Console.WriteLine();
            }

            Console.WriteLine(JsonConvert.SerializeObject(house, Formatting.Indented));

            //Console.WriteLine(JsonSerializer.Serialize(house, new JsonSerializerOptions() { WriteIndented = true }));

            //Console.WriteLine($"TotalRevenue: {rose1.TotalRevenue()}");
            //Console.WriteLine($"ManagerCut: {house.ManagerCut}");
        }
    }
}
