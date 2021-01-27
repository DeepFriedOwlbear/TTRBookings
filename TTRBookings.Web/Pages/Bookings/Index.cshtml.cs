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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public IList<Booking> Bookings { get; set; }

        //dear dependency Booking, if you know how to create an XYZ,
        //then please give me an XYZ.
        public IndexModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public void OnGet()
        {
            //Bookings = repository.List<Booking>();
            Bookings = repository.ListWithIncludes<Booking>(_ => true, _ => _.Room, _ => _.Rose, _ => _.Tier, _ => _.TimeSlot);
        }
    }
}
