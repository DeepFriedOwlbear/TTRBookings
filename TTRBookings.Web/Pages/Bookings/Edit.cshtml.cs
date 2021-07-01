using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly IRepository repository;
        
        [BindProperty]
        public BookingVM BookingVM { get; set; }

        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem>();
        public List<SelectListItem> StaffList { get; } = new List<SelectListItem>();

        public EditModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            //Load booking matching Id
            var booking = repository.ReadEntryWithIncludes<Booking>(id, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);
            //Populate drop-down lists
            PopulateLists(booking.Id);
            //convert booking to bookingvm here;
            BookingVM = BookingVM.Create(booking);
        }

        private void PopulateLists(Guid bookingId)
        {
            RoomList.AddRange(SelectListHelper.PopulateList(
                repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));

            StaffList.AddRange(SelectListHelper.PopulateList(
                repository.List<Core.Entities.Staff>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));
        }
    }
}
