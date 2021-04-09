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
    //TODO - FORM POST VERSION
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
            if (repository.DeleteEntry(repository.ReadEntry<Booking>(id)) == true)
            {
                return new JsonResult(new { Success = true });
            }
            else 
            { 
                return new JsonResult(new { Success = false }); 
            }

            //return RedirectToPage("/Bookings/Index");

            //if (DateTime.Now.Second % 2 == 0)
            //{
            //    return new JsonResult(new { Success = false });
            //}
            //return new JsonResult(new { Success = true });
        }

        public IActionResult OnPost()
        {
            //TODO - Work with posted FormData here.
            Guid bookingId = Guid.Parse(Request.Form["bookingId"]);
            
            if (DateTime.Now.Second % 2 == 0)
            {
                return new JsonResult(new { Success = false });
            }
            return new JsonResult(new { Success = true });
        }
    }
}
