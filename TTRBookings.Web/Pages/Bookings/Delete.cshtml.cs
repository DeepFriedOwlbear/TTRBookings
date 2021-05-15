using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        [BindProperty]
        public Guid bookingId { get; set; }

        //dear dependency Booking, if you know how to create an XYZ,
        //then please give me an XYZ.
        public DeleteModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public IActionResult OnGet(Guid id)
        {
            //return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Booking>(id)) });

            //return RedirectToPage("/Bookings/Index");
            return null;
        }

        public IActionResult OnPost()
        {
            //bookingId = Guid.Parse(Request.Form["bookingId"]);

            //if (repository.DeleteEntry(repository.ReadEntry<Booking>(bookingId)) == true)
            //{
            //    return new JsonResult(new { Success = true });
            //}
            //else
            //{
            //    return new JsonResult(new { Success = false });
            //}
            return null;
        }
    }
}