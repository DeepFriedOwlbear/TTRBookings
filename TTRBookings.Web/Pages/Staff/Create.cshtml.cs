using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CreateModel : PageModel
    {
        //private readonly ILogger<CreateModel> _logger;
        private readonly IRepository repository;

        [BindProperty] public StaffVM StaffVM { get; set; }

        //public CreateModel(ILogger<CreateModel> logger, IRepository repository)
        //{
        //    _logger = logger;
        //    this.repository = repository;
        //}

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Core.Entities.Staff staff = new Core.Entities.Staff(StaffVM.Name);
            staff.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            repository.CreateEntry(staff);

            return Redirect("/Staff");
        }
    }
}
