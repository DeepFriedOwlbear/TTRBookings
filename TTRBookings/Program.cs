using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using TTRBookings.Core.Entities;

namespace TTRBookings
{
    public class Program
    {
        //TODO - Check if Rose or Room is booked in AddBooking() before adding it to Bookings & RoomBookings.
        //TODO - DisplayBookings() should use the DB instead of the house object.

        static void Main(string[] args)
        {
            //RepositorySeed.SeedDatabase();

            //House house = Repository.ListWithIncludes<House>(_ => true, _ => _.Managers, _ => _.Rooms, _ => _.TierRates, _ => _.Bookings[0].TimeSlot, _ => _.Roses[0].Tiers)
            //    .FirstOrDefault();

            //Repository.CreateEntry(house.CreateRose("Rose3"));
            //var rose3 = Repository.ReadEntry<Rose>(house.Roses.Last().Id);
            //rose3.Name = "Rose3_Modified";
            //Repository.UpdateEntry(rose3);
            //Repository.DeleteEntry(rose3);


            //IList<Booking> bod = Repository.ListWithIncludes<Booking>(r=>r.Rose == rose1, r=>r.Rose,r=>r.Room,r=>r.Tier, r=>r.TimeSlot);

            //DisplayBookings(house);
        }

        private static void DisplayBookings(House house)
        {
            //TimeSlot lastTimeslot = null;
            //using var context = new TTRBookingsContext();

            //foreach (Booking booking in house.Bookings.OrderBy(k => k.TimeSlot.Start))
            //{
            //    if (lastTimeslot == null || lastTimeslot != booking.TimeSlot)
            //    {
            //        Console.WriteLine($"{booking.TimeSlot.Start:yyyy-MM-dd HH:mm} - {booking.TimeSlot.End:HH:mm}");
            //        Console.WriteLine($"{booking.Room.Name} > {booking.Rose.Name}");
            //    }
            //    else
            //    {
            //        Console.WriteLine();
            //    }

            //    lastTimeslot = booking.TimeSlot;
            //    Console.WriteLine();
            //}

            //show house object
            //Console.WriteLine(JsonConvert.SerializeObject(house, Formatting.Indented));

            //Console.WriteLine(JsonSerializer.Serialize(house, new JsonSerializerOptions() { WriteIndented = true }));

            //Console.WriteLine($"TotalRevenue: {rose1.TotalRevenue()}");
            //Console.WriteLine($"ManagerCut: {house.ManagerCut}");
        }
    }
}
