using System;


namespace TTRBookings.Core.Entities
{
    public class Manager : BaseEntity
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public Manager(string name)
        {
            Name = name;
        }
    }
}
