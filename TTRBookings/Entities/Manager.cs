using System;
using TTRBookings.Data;

namespace TTRBookings.Entities
{
    public class Manager : BaseEntity
    {
        public string Name { get; set; }
        public Manager(string name)
        {
            Name = name;
        }
    }
}
