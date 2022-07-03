using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages.Rooms;

public class IndexModel : PageModel
{
    private readonly IRepository<Room> _rooms;
    public IList<Room> Rooms { get; set; }

    public IndexModel(IRepository<Room> rooms)
    {
        _rooms=rooms;
    }

    public async Task OnGetAsync()
    {
        Rooms = await _rooms.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync();
    }
}
