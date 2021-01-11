using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRBookings
{
    public class House
    {
        public decimal RoseCut { get; set; } = 0.7M; //0.7M = 70%
        public decimal HouseCut { get; set; } = 0.3M; //0.3M = 30%

        public IList<Rose> Roses { get; set; } = new List<Rose>();
        public IList<Manager> Managers { get; set; } = new List<Manager>();
        public IList<Room> Rooms { get; set; } = new List<Room>();

        public IDictionary<TimeSlot, IList<Rose>> Bookings = new Dictionary<TimeSlot, IList<Rose>>();
        public IDictionary<TimeSlot, IList<Room>> RoomBookings = new Dictionary<TimeSlot, IList<Room>>();

        public decimal ManagerCut => Calculator.DoManagerCalculation(Managers.Count, TotalRoseRevenue());

        public Rose CreateRose(string name)
        {
            Rose rose = new Rose(name);
            Roses.Add(rose);
            return rose;
        }

        public Manager CreateManager(string name)
        {
            Manager manager = new Manager(name);
            Managers.Add(manager);
            return manager;
        }

        public Room CreateRoom(string name)
        {
            Room room = new Room(name);
            Rooms.Add(room);
            return room;
        }

        public void AddBooking(Rose rose, int tier, Room room, TimeSlot timeslot, int duration)
        {
            for (int i = 0; i < duration; i++)
            {
                TimeSlot tempTimeslot = new TimeSlot(timeslot.Start.Add(Calculator.TimeUnit * i));
                AddSingleSlotBooking(tempTimeslot, rose, room);
            }
            rose.AddTier(tier, duration);
        }

        private void AddSingleSlotBooking(TimeSlot timeslot, Rose rose, Room room)
        {
            var existingRoseSlot = Bookings.FirstOrDefault(b => b.Key.Start == timeslot.Start);
            var existingRoomSlot = RoomBookings.FirstOrDefault(b => b.Key.Start == timeslot.Start);

            //Add Rose booking + timeslot
            if (existingRoseSlot.Key != null)
            {
                var bookedRoses = existingRoseSlot.Value;
                bookedRoses.Add(rose);
            }
            else
            {
                List<Rose> bookedRoses = new List<Rose>();
                bookedRoses.Add(rose);
                Bookings.Add(timeslot, bookedRoses);
            }

            //Add Room booking + timeslot
            if (existingRoomSlot.Key != null)
            {
                var bookedRooms = existingRoomSlot.Value;
                bookedRooms.Add(room);
            }
            else
            {
                List<Room> bookedRooms = new List<Room>();
                bookedRooms.Add(room);
                RoomBookings.Add(timeslot, bookedRooms);
            }
        }

        private decimal TotalRoseRevenue()
        {
            return Roses.Sum(a => a.TotalRevenue());
        }
    }
}
