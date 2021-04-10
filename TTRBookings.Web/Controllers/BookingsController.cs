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

        //dear dependency Booking, if you know how to create an XYZ,
        //then please give me an XYZ.
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
            //TODO - Work with posted FormData here.
            //Guid bookingId = Guid.Parse(Request.Form["bookingId"]);
            
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Booking>(booking.BookingId)) });
        }

        //public IActionResult OnGet(Guid id)
        //{
        //    return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Booking>(id)) });

        //    //return RedirectToPage("/Bookings/Index");

        //    //if (DateTime.Now.Second % 2 == 0)
        //    //{
        //    //    return new JsonResult(new { Success = false });
        //    //}
        //    //return new JsonResult(new { Success = true });
        //}

        //public IActionResult OnPost()
        //{
        //    //TODO - Work with posted FormData here.
        //    bookingId = Guid.Parse(Request.Form["bookingId"]);

        //    if (repository.DeleteEntry(repository.ReadEntry<Booking>(bookingId)) == true)
        //    {
        //        return new JsonResult(new { Success = true });
        //    }
        //    else
        //    {
        //        return new JsonResult(new { Success = false });
        //    }
        //}
    }

    public sealed class BookingDeleteDTO
    {
        public Guid BookingId { get; set; }
    }
}
