using System;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; }
        public Room(string name)
        {
            Name = name;
        }
    }
}
