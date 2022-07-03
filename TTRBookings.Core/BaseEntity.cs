using System;

namespace TTRBookings.Core;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsArchived { get; set; }
}
