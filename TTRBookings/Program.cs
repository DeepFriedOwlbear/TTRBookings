using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TTRBookings
{
    public class Program
    {
        static void Main(string[] args)
        {
            //--input for calculations
            House house = new House();

            Rose rose1 = house.CreateRose("Rose1");
            Rose rose2 = house.CreateRose("Rose2");

            house.CreateManager("Alice");
            house.CreateManager("Bob");

            house.AddBooking(rose1, TierList.Tier1, new TimeSlot(new DateTime(2021, 1, 4, 20, 00, 00, DateTimeKind.Utc)), 1);
            house.AddBooking(rose2, TierList.Tier2, new TimeSlot(new DateTime(2021, 1, 4, 19, 30, 00, DateTimeKind.Utc)), 2);
            //--end of input for calculation

            Console.WriteLine(JsonConvert.SerializeObject(house, Formatting.Indented));

            //Console.WriteLine(JsonSerializer.Serialize(house, new JsonSerializerOptions() { WriteIndented = true }));
            //Console.WriteLine(JsonSerializer.Serialize(house.Bookings, new JsonSerializerOptions() { WriteIndented = true }));

            //show calculations
            //Console.WriteLine($"TotalRevenue: {rose1.TotalRevenue()}");
            //Console.WriteLine($"ManagerCut: {house.ManagerCut}");

        }


    }
}
