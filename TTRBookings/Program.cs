using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TTRBookings
{
    public class Program
    {
        //TODO - Check if Rose or Room is booked in AddBooking() before adding it to Bookings & RoomBookings.

        static void Main(string[] args)
        {
            //--input for calculations
            House house = new House();

            Rose rose1 = house.CreateRose("Rose1");
            Rose rose2 = house.CreateRose("Rose2");

            house.CreateManager("Alice");
            house.CreateManager("Bob");

            Room room1 = house.CreateRoom("Room1");
            Room room2 = house.CreateRoom("Room2");
            Room room3 = house.CreateRoom("Room3");
            Room room4 = house.CreateRoom("Room4");

            house.AddBooking(rose1, TierList.Tier1, room1, new TimeSlot(new DateTime(2021, 1, 4, 20, 00, 00, DateTimeKind.Utc)), 1);
            house.AddBooking(rose2, TierList.Tier2, room2, new TimeSlot(new DateTime(2021, 1, 4, 19, 30, 00, DateTimeKind.Utc)), 2);
            house.AddBooking(rose1, TierList.Tier1, room3, new TimeSlot(new DateTime(2021, 1, 4, 22, 00, 00, DateTimeKind.Utc)), 3);
            house.AddBooking(rose2, TierList.Tier2, room4, new TimeSlot(new DateTime(2021, 1, 4, 23, 30, 00, DateTimeKind.Utc)), 4);
            //--end of input for calculation

            //Console.WriteLine(JsonConvert.SerializeObject(house.RoomBookings, Formatting.Indented));

            /*foreach (var roomKey in house.RoomBookings.Keys)
            {
                List<Room> bookedRooms = new List<Room>();
                var existingRoomSlot = house.RoomBookings.FirstOrDefault(b => b.Key.Start == roomKey.Start);
                Console.WriteLine($"Timeslot: {roomKey.Start} - {existingRoomSlot.Value.}");
            }*/

            //Console.WriteLine(JsonSerializer.Serialize(house, new JsonSerializerOptions() { WriteIndented = true }));
            //Console.WriteLine(JsonSerializer.Serialize(house.Bookings, new JsonSerializerOptions() { WriteIndented = true }));

            //show calculations
            //Console.WriteLine($"TotalRevenue: {rose1.TotalRevenue()}");
            //Console.WriteLine($"ManagerCut: {house.ManagerCut}");
        }
    }
}
