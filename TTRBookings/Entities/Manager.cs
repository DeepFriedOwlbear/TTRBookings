using System;

namespace TTRBookings.Entities
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Manager(string name)
        {
            Name = name;
        }
    }
}
