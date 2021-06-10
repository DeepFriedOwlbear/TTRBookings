using System;
using System.Collections.Generic;
using System.Linq;


namespace TTRBookings.Core.Entities
{
    public class House : BaseEntity
    {
        public string Name { get; set; }

        public decimal StaffCut { get; set; } = 0.7M; //0.7M = 70%
        public decimal HouseCut { get; set; } = 0.3M; //0.3M = 30%

        public IList<Staff> Staff { get; set; } = new List<Staff>();
        public IList<Manager> Managers { get; set; } = new List<Manager>();
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public decimal ManagerCut => Calculator.DoManagerCalculation(Managers.Count, TotalStaffRevenue());

        public House(string name)
        {
            Name = name;
        }

        public Staff CreateStaff(string name)
        {
            Staff staff = new Staff(name);
            staff.HouseId = Id;
            Staff.Add(staff);
            
            return staff;
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

        public void AddBooking(Staff staff, int tierRate, Room room, TimeSlot timeslot)
        {
            Tier tier = new Tier(tierRate)
            {
                Discount = 1,
                Unit = (int)((timeslot.End - timeslot.Start) / Calculator.TimeUnit)
            };
            Booking booking = Booking.Create(Id, staff, tier, room, timeslot);

            Bookings.Add(booking);

            staff.AddTier(tier);
        }

        public void RemoveBooking(Booking booking)
        {
            Booking currentBooking = Bookings.FirstOrDefault(b => b == booking);
            Bookings.Remove(currentBooking);

            Staff currentStaff = Staff.FirstOrDefault(r => r == booking.Staff);
            currentStaff.RemoveTier(booking.Tier);
        }

        public void UpdateBooking(Booking booking, Tier tier, Staff staff, Room room, TimeSlot timeSlot)
        {
            //Recalculate Tier
            tier.Discount = 1;
            tier.Unit = (int)((timeSlot.End - timeSlot.Start) / Calculator.TimeUnit);

            //Add and Remove tiers
            booking.Staff.RemoveTier(booking.Tier);
            staff.AddTier(tier);

            //update booking
            booking.Update(staff)
                .Update(room)
                .Update(tier)
                .Update(timeSlot);
        }

        private decimal TotalStaffRevenue()
        {
            return Staff.Sum(a => a.TotalRevenue());
        }
    }
}
