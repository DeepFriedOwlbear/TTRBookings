using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

            //convert booking to bookingvm here;
            StaffVM = StaffVM.Create(staff);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
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
