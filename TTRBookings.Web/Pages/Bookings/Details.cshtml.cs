using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;

        public Booking Booking { get; set; }

        public DetailsModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            //what was the goal for this page?
            //What do we need to ask our dearest dependency container?


            Booking = repository.ReadEntryWithIncludes<Booking>(id, _ => _.Room, _ => _.Rose, _ => _.TimeSlot, _ => _.Tier);

            //Load booking where id matches from database
            //pass information to Details.cshtml

        }
    }
}
