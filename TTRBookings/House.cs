using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRBookings
{
    public class House
    {
        public decimal RoseCut { get; set; } = 0.7M; //0.7M = 70%
        public decimal HouseCut { get; set; } = 0.3M; //0.3M = 30%

        public IList<Manager> Managers { get; set; } = new List<Manager>();
        public IList<Rose> Roses { get; set; } = new List<Rose>();
        public IDictionary<TimeSlot, IList<Rose>> Bookings = new Dictionary<TimeSlot, IList<Rose>>();
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

        public void AddBooking(Rose rose, int tier, TimeSlot timeslot, int duration)
        {
            for (int i = 0; i < duration; i++)
            {
                TimeSlot tempTimeslot = new TimeSlot(timeslot.Start.Add(Calculator.TimeUnit * i));
                AddSingleSlotBooking(tempTimeslot, rose);
            }
            rose.AddTier(tier, duration);
        }

        private void AddSingleSlotBooking(TimeSlot timeslot, Rose rose)
        {
            var existingSlot = Bookings.FirstOrDefault(b => b.Key.Start == timeslot.Start);
            
            if (existingSlot.Key != null)
            {
                var bookedRoses = existingSlot.Value;
                bookedRoses.Add(rose);
            }
            else
            {
                List<Rose> bookedRoses = new List<Rose>();
                bookedRoses.Add(rose);
                Bookings.Add(timeslot, bookedRoses);
            }
        }

        private decimal TotalRoseRevenue()
        {
            return Roses.Sum(a => a.TotalRevenue());
        }
    }
}
