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
    public class CreateModel : PageModel
    {
        private readonly IRepository repository;

        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> StaffList { get; } = new List<SelectListItem> { };

        [BindProperty] public BookingVM BookingVM { get; set; }

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            //populate drop-down lists
            PopulateLists(null);
        }

        private void PopulateLists(Guid? bookingId)
        {
            //Load Lists before returning the Page
            RoomList.AddRange(SelectListHelper.PopulateList<Room>(
                repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));

            StaffList.AddRange(SelectListHelper.PopulateList<Core.Entities.Staff>(
                repository.List<Core.Entities.Staff>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));
        }
    }
}
