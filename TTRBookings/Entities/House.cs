using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRBookings.Entities
{
    public class House
    {
        public Guid Id { get; set; }
        public decimal RoseCut { get; set; } = 0.7M; //0.7M = 70%
        public decimal HouseCut { get; set; } = 0.3M; //0.3M = 30%

        public IList<Rose> Roses { get; set; } = new List<Rose>();
        public IList<Manager> Managers { get; set; } = new List<Manager>();
        public IList<Room> Rooms { get; set; } = new List<Room>();

        public IDictionary<TimeSlot, IList<Room>> Bookings = new Dictionary<TimeSlot, IList<Room>>();

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

        public void AddBooking(Rose rose, TierRate tier, Room room, TimeSlot timeslot, int duration)
        {
            for (int i = 0; i < duration; i++)
            {
                TimeSlot tempTimeslot = new TimeSlot(timeslot.Start.Add(Calculator.TimeUnit * i));
                rose.AddTier(tier);
                room.Rose = rose;
                AddSingleSlotBooking(tempTimeslot, room);
            }
        }

        private void AddSingleSlotBooking(TimeSlot timeslot, Room room)
        {
            var existingRoomSlot = Bookings.FirstOrDefault(b => b.Key.Start == timeslot.Start);

            if (existingRoomSlot.Key != null)
            {
                var bookedRooms = existingRoomSlot.Value;
                bookedRooms.Add(room);
            }
            else
            {
                List<Room> bookedRooms = new List<Room>();
                bookedRooms.Add(room);
                Bookings.Add(timeslot, bookedRooms);
            }
        }

        private decimal TotalRoseRevenue()
        {
            return Roses.Sum(a => a.TotalRevenue());
        }
    }
}
