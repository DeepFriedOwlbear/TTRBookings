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

namespace TTRBookings.Web.Pages.Roses
{
    public class EditModel : PageModel
    {
        //private readonly ILogger<CreateModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public RoseVM RoseVM { get; set; }

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
            var rose = repository.ReadEntry<Rose>(id);

            //convert booking to bookingvm here;
            RoseVM = RoseVM.Create(rose);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Rose rose = repository.ReadEntry<Rose>(RoseVM.Id);
            rose.Name = RoseVM.Name;

            //store in database
            repository.UpdateEntry(rose);

            //return/redirect user to somewhere
            return RedirectToPage("/Roses/Edit", new { rose.Id });
        }
    }
}
