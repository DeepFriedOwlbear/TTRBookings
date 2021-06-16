using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Controllers
{
    [Route("api/[controller]")] //< =  https://localhost:12345/api/bookings
    [ApiController]
    public class BookingsController : ControllerBase
    {   
        private readonly ILogger<BookingsController> _logger;
        private readonly IRepository repository;

        public BookingsController(ILogger<BookingsController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("delete")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Delete(BookingDeleteDTO booking)
        {
            //retrieve originals from database.
            Booking bookingDB = repository.ReadEntryWithIncludes<Booking>(booking.BookingId, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);
            House house = repository.ReadEntryWithIncludes<House>(bookingDB.HouseId, _ => _.Managers, _ => _.Rooms, _ => _.Staff, _ => _.Bookings, _ => _.Bookings[0].Tier, _ => _.Bookings[0].TimeSlot);

            //remove booking from house
            house.RemoveBooking(bookingDB);

            //store in database and return success state
            return new JsonResult(new { Success = repository.UpdateEntry(house) });
        }
    }

    public sealed class BookingDeleteDTO
    {
        public Guid BookingId { get; set; }
    }
}
