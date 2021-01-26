using System;


namespace TTRBookings.Core.Entities
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
