using System;
using System.Collections.Generic;
using System.Linq;


namespace TTRBookings.Core.Entities
{
    public class House : BaseEntity
    {
        public string Name { get; set; }

        public decimal RoseCut { get; set; } = 0.7M; //0.7M = 70%
        public decimal HouseCut { get; set; } = 0.3M; //0.3M = 30%

        public IList<Rose> Roses { get; set; } = new List<Rose>();
        public IList<Manager> Managers { get; set; } = new List<Manager>();
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<TierRate> TierRates { get; set; } = new List<TierRate>();
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public decimal ManagerCut => Calculator.DoManagerCalculation(Managers.Count, TotalRoseRevenue());

        public House(string name)
        {
            Name = name;
        }

        public Rose CreateRose(string name)
        {
            Rose rose = new Rose(name);
            rose.HouseId = Id;
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

        public TierRate CreateTierRate(string tier, int value)
        {
            TierRate tierRate = new TierRate(tier, value);
            TierRates.Add(tierRate);
            return tierRate;
        }

        public void AddBooking(Rose rose, TierRate tierRate, Room room, TimeSlot timeslot)
        {
            Tier tier = new Tier()
            {
                Discount = 1,
                Rate = tierRate.Value,
                Unit = (int)((timeslot.End - timeslot.Start) / Calculator.TimeUnit)
            };
            Booking booking = Booking.Create(Id, rose, tier, room, timeslot);

            Bookings.Add(booking);

            rose.AddTier(tier);
        }

        private decimal TotalRoseRevenue()
        {
            return Roses.Sum(a => a.TotalRevenue());
        }
    }
}
