using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages.Managers;

public class IndexModel : PageModel
{
    private readonly IRepository<Manager> _managers;

    public IList<Manager> Managers { get; set; }

    public IndexModel(IRepository<Manager> managers)
    {
        _managers=managers;
    }

    public async Task OnGetAsync()
    {
        Managers = await _managers.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync();
    }
}
