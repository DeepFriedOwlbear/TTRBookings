using System;

namespace TTRBookings.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Room(string name)
        {
            Name = name;
        }
    }
}
