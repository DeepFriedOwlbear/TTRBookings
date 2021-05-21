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
        //private readonly ILogger<DeleteModel> _logger;
        private readonly IRepository repository;

        [BindProperty]
        public Guid bookingId { get; set; }

        //dear dependency Booking, if you know how to create an XYZ,
        //then please give me an XYZ.

        //public DeleteModel(ILogger<DeleteModel> logger, IRepository repository)
        //{
        //    _logger = logger;
        //    this.repository = repository;
        //}
        
        public DeleteModel(IRepository repository)
        {
            this.repository = repository;
        }
    }
}