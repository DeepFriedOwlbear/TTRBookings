using System;
using System.Diagnostics;

namespace TTRBookings.Core.Entities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Manager : BaseEntity
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public Manager(string name)
        {
            Name = name;
        }

        private string GetDebuggerDisplay()
        {
            return Name;
        }
    }
}
