using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Rooms;

public class CreateModel : PageModel
{
    [BindProperty] public RoomVM RoomVM { get; set; }

    public CreateModel()
    {

    }
}
