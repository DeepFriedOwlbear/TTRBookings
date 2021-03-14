using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Roses
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public DeleteModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public IActionResult OnGet(Guid id)
        {
            //TODO - Can't delete entry because it is tied to a booking.
            //Might be better to set an IsDeleted flag so it doesn't break existing bookings
            repository.DeleteEntry(repository.ReadEntry<Rose>(id));
            return RedirectToPage("/Roses/Index");
        }
    }
}
