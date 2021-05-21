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

namespace TTRBookings.Web.Pages.Managers
{
    public class EditModel : PageModel
    {
        //private readonly ILogger<CreateModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public ManagerVM ManagerVM { get; set; }

        //public EditModel(ILogger<IndexModel> logger, IRepository repository)
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
            var manager = repository.ReadEntry<Manager>(id);

            //convert booking to bookingvm here;
            ManagerVM = ManagerVM.Create(manager);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Manager manager = repository.ReadEntry<Manager>(ManagerVM.Id);
            manager.Name = ManagerVM.Name;

            //store in database
            repository.UpdateEntry(manager);

            //return/redirect user to somewhere
            return RedirectToPage("/Managers/Edit", new { manager.Id });
        }
    }
}
