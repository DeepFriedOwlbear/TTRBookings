using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TTRBookings.Core.Entities;

namespace TTRBookings.Web.Pages.Bookings;

public class IndexModel : PageModel
{
    public IList<Booking> Bookings { get; set; } = new List<Booking>();

    public IndexModel()
    {

    }

    public void OnGet()
    {
    
    }
}
