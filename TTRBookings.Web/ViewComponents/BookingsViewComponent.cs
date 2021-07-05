using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.ViewComponents
{
    public class BookingsViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public BookingsViewComponent(IRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke(Guid repositoryId, string repositoryFilter)
        {
            Bookings = repositoryFilter switch
            {
                "staff" => repository.ListWithIncludes<Booking>(
                    _ => _.Staff.Id == repositoryId,
                    _ => _.Room, _ => _.Staff, _ => _.Tier, _ => _.TimeSlot),

                "room" => repository.ListWithIncludes<Booking>(
                    _ => _.Room.Id == repositoryId,
                    _ => _.Room, _ => _.Staff, _ => _.Tier, _ => _.TimeSlot),

                _ => repository.ListWithIncludes<Booking>(
                    _ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")),
                    _ => _.Room, _ => _.Staff, _ => _.Tier, _ => _.TimeSlot),
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
}
