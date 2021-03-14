using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> RoseList { get; } = new List<SelectListItem> { };

        [BindProperty] public BookingVM BookingVM { get; set; }

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            RoomList.AddRange(SelectListHelper.PopulateList<Room>(repository.List<Room>(), e => e.Name));
            RoseList.AddRange(SelectListHelper.PopulateList<Rose>(repository.List<Rose>(), e => e.Name));
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Tier tier = new Tier()
            {
                Unit = BookingVM.Tier.Unit,
                Rate = BookingVM.Tier.Rate
            };
            
            Booking booking = Booking.Create(repository.ReadEntry<Rose>(BookingVM.Rose.Id), tier, repository.ReadEntry<Room>(BookingVM.Room.Id), new TimeSlot(BookingVM.TimeSlot.Start, BookingVM.TimeSlot.End));

            //store in database
            repository.CreateEntry(booking);

            //return/redirect user to somewhere
            return RedirectToPage("/Bookings/Details", new { booking.Id });
        }
    }


}
