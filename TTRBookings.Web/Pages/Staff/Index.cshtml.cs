using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages.Staff;

public class IndexModel : PageModel
{
    private readonly IRepository<Core.Entities.Staff> _staff;

    public IList<Core.Entities.Staff> Staff { get; set; }

    public IndexModel(IRepository<Core.Entities.Staff> staff)
    {
        _staff=staff;
    }

    public async Task OnGetAsync()
    {
        Staff = await _staff.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync();
    }
}
