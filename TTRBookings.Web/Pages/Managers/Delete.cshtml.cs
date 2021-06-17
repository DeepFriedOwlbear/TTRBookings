using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Managers
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository repository;

        public DeleteModel(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult OnGet(Guid id)
        {
            repository.DeleteEntry(repository.ReadEntry<Manager>(id));
            return RedirectToPage("/Managers/Index");
        }
    }
}
