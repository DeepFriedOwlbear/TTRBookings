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

namespace TTRBookings.Web.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public RoomVM RoomVM { get; set; }

        public EditModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            var room = repository.ReadEntry<Room>(id);

            //convert booking to bookingvm here;
            RoomVM = RoomVM.Create(room);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Room room = repository.ReadEntry<Room>(RoomVM.Id);
            room.Name = RoomVM.Name;

            //store in database
            repository.UpdateEntry(room);

            //return/redirect user to somewhere
            return RedirectToPage("/Rooms/Edit", new { room.Id });
        }
    }
}
