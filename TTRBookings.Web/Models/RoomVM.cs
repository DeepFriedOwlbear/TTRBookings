using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class RoomVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public static RoomVM Create(Room room)
    {
        return new RoomVM()
        {
            Id = room.Id,
            Name = room.Name
        };
    }
}