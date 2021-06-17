using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty]
        public Guid bookingId { get; set; }
        
        public DeleteModel(IRepository repository)
        {
            this.repository = repository;
        }
    }
}