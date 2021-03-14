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
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Roses
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;

        [BindProperty] public RoseVM RoseVM { get; set; }
        [BindProperty] public House house { get; set; }

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            //TODO - House probably needs its own HouseVM
            //TODO - Currently assigning new Roses to the only house in the DB, add a way to assign which house is currently being worked on.
            house = repository.List<House>().FirstOrDefault();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Rose rose = new Rose(RoseVM.Name);
            rose.HouseId = house.Id;

            //store in database
            repository.CreateEntry(rose);

            //return/redirect user to somewhere
            return RedirectToPage("/Roses/Details", new { rose.Id });
        }
    }
}
