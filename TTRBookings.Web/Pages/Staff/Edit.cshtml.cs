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
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Staff
{
    public class EditModel : PageModel
    {
        //private readonly ILogger<CreateModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public StaffVM StaffVM { get; set; }
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

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
            var staff = repository.ReadEntry<Core.Entities.Staff>(id);
            StaffVM = StaffVM.Create(staff);

            //load bookings for the staff
            Bookings = repository.ListWithIncludes<Booking>(
                _ => _.Staff.Id == staff.Id,
                _ => _.Room, _ => _.Staff, _ => _.Tier, _ => _.TimeSlot);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                //load bookings for the staff
                Bookings = repository.ListWithIncludes<Booking>(
                    _ => _.Staff.Id == StaffVM.Id,
                    _ => _.Room, _ => _.Staff, _ => _.Tier, _ => _.TimeSlot);

                return Page();
            }

            Core.Entities.Staff staff = repository.ReadEntry<Core.Entities.Staff>(StaffVM.Id);
            staff.Name = StaffVM.Name;

            //store in database
            repository.UpdateEntry(staff);

            //return/redirect user to somewhere
            return RedirectToPage("/Staff/Edit", new { staff.Id });
        }
    }
}
