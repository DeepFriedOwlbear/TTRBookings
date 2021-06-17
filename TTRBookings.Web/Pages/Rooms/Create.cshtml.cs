using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Rooms
{
    public class CreateModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty] public RoomVM RoomVM { get; set; }

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

            Room room = new Room(RoomVM.Name);
            room.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            repository.CreateEntry(room);

            return Redirect("/Rooms");
        }
    }
}
