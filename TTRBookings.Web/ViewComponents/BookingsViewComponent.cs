using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Core.QueryExtensions;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.ViewComponents;

public class BookingsViewComponent : ViewComponent
{
    private readonly IRepository<Booking> _bookings;
    public IList<Booking> Bookings { get; set; } = new List<Booking>();

    public BookingsViewComponent(
        IRepository<Booking> bookings)
    {
        _bookings=bookings;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid repositoryId, string repositoryFilter)
    {
        var bookingsQuery = _bookings.WithIncludes();

        Bookings = repositoryFilter switch
        {
            "staff" => await bookingsQuery.Where(x => x.Staff.Id == repositoryId).ToListAsync(),
            "room"  => await bookingsQuery.Where(x => x.Room.Id == repositoryId).ToListAsync(),
                  _ => await bookingsQuery.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync()
        };

        return View(new BookingsList(Bookings) { RepositoryFilter = repositoryFilter });
    }
}

public class BookingsList
{
    public IList<Booking> Bookings { get; } = new List<Booking> { };

    public string RepositoryFilter { get; set; }

    public BookingsList(IList<Booking> bookings)
    {
        Bookings = bookings;
    }
}
