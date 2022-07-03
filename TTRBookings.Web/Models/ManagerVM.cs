using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Models;

public class ManagerVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public static ManagerVM Create(Manager manager)
    {
        return new ManagerVM()
        {
            Id = manager.Id,
            Name = manager.Name
        };
    }
}