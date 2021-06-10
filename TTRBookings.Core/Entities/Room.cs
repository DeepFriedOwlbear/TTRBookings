using System;
using System.Diagnostics;

namespace TTRBookings.Core.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Room : BaseEntity
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public Room(string name)
        {
            Name = name;
        }

        private string GetDebuggerDisplay()
        {
            return Name;
        }
    }
}
