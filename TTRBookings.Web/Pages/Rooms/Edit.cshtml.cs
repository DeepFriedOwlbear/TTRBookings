using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Rooms;

public class EditModel : PageModel
{
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
}
