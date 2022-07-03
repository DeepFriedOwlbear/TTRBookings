using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Managers;

public class CreateModel : PageModel
{
    [BindProperty] public ManagerVM ManagerVM { get; set; }

    public CreateModel()
    {

    }
}
