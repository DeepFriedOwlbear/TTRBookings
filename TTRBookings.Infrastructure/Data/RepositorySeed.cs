using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TTRBookings.Authentication.Data;
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

            context.Users.Add(new User("TestUser", "123123"));
            context.Users.Add(new User("Admin", "123123"));

            context.SaveChanges();
        }

        private static House CreateHouse()
        {
            //--input for calculations
            House house = new House("House_1");

            Staff staff1 = house.CreateStaff("1_Staff1");
            Staff staff2 = house.CreateStaff("1_Staff2");

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

            house.AddBooking(staff1, 39.99M, room1, timeslot1);
            house.AddBooking(staff2, 59.99M, room2, timeslot2);
            house.AddBooking(staff1, 59.99M, room1, timeslot3);
            house.AddBooking(staff2, 39.99M, room3, timeslot4);

            return house;
            //--end of input for calculation
        }

        private static House CreateHouse2()
        {
            //--input for calculations
            House house = new House("House_2");

            Staff staff1 = house.CreateStaff("2_Staff1");
            Staff staff2 = house.CreateStaff("2_Staff2");
            Staff staff3 = house.CreateStaff("2_Staff3");

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

            house.AddBooking(staff1, 19.95M, room1, timeslot1);
            house.AddBooking(staff2, 39.99M, room2, timeslot2);
            house.AddBooking(staff3, 69.99M, room3, timeslot3);
            house.AddBooking(staff2, 19.95M, room4, timeslot4);

            return house;
            //--end of input for calculation
        }
    }
}