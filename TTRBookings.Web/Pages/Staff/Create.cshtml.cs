using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Staff;

public class CreateModel : PageModel
{
    [BindProperty] public StaffVM StaffVM { get; set; }

    public CreateModel()
    {

    }
}
