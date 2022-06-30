using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Rooms;

public class CreateModel : PageModel
{
    private readonly IRepository repository;

    [BindProperty] public RoomVM RoomVM { get; set; }

    public CreateModel(IRepository repository)
    {
        this.repository = repository;
    }
}
