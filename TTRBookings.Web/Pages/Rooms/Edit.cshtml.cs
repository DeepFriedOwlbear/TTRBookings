using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Rooms;

public class EditModel : PageModel
{
    private readonly IRepository<Room> _rooms;

    [BindProperty]
    public RoomVM RoomVM { get; set; }

    public EditModel(IRepository<Room> rooms)
    {
        _rooms = rooms;
    }

    public async Task OnGetAsync(Guid id)
    {
        var room = await _rooms.GetByIdAsync(id);

        //convert booking to bookingvm here;
        RoomVM = RoomVM.Create(room);
    }
}
