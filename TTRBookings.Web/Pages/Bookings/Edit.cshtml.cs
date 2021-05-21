using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class EditModel : PageModel
    {
        //private readonly ILogger<EditModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public BookingVM BookingVM { get; set; }

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem>();
        public List<SelectListItem> RoseList { get; } = new List<SelectListItem>();

        //public EditModel(ILogger<EditModel> logger, IRepository repository)
        //{
        //    _logger = logger;
        //    this.repository = repository;
        //}

        public EditModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            var booking = repository.ReadEntryWithIncludes<Booking>(id, _ => _.Room, _ => _.Rose, _ => _.TimeSlot, _ => _.Tier);

            RoomList.AddRange(SelectListHelper.PopulateList<Room>(
                repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e=>e.Name,
                booking.Room.Id
            ));

            RoseList.AddRange(SelectListHelper.PopulateList<Rose>(
                repository.List<Rose>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))), 
                e=>e.Name, 
                booking.Rose.Id
            ));

            //convert booking to bookingvm here;
            BookingVM = BookingVM.Create(booking);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //retrieve original from database.
            Booking booking = repository.ReadEntryWithIncludes<Booking>(BookingVM.Id, _ => _.Room, _ => _.Rose, _ => _.TimeSlot, _ => _.Tier);

            //assign bookingVM values to booking
            booking.Room = repository.ReadEntry<Room>(BookingVM.Room.Id);
            booking.Rose = repository.ReadEntry<Rose>(BookingVM.Rose.Id);
            booking.Tier.Rate = BookingVM.Tier.Rate;
            booking.TimeSlot.Start = BookingVM.TimeSlot.Start;
            booking.TimeSlot.End = BookingVM.TimeSlot.End;

            //store in database
            repository.UpdateEntry(booking);

            //return/redirect user to somewhere
            return RedirectToPage("/Bookings/Edit", new { booking.Id });
        }
    }
}
