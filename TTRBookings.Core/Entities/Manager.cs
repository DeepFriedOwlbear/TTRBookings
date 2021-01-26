using System;


namespace TTRBookings.Core.Entities
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
