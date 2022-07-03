using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly IRepository<Room> _rooms;
    private readonly IRepository<Core.Entities.Staff> _staff;

    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
    public List<SelectListItem> StaffList { get; } = new List<SelectListItem> { };

    [BindProperty] public BookingVM BookingVM { get; set; }

    public CreateModel(
        IRepository<Room> rooms, 
        IRepository<Core.Entities.Staff> staff)
    {
        _rooms = rooms;
        _staff = staff;
    }

    public async Task OnGetAsync()
    {
        //populate drop-down lists
        await PopulateLists(null);
    }

    private async Task PopulateLists(Guid? bookingId)
    {
        //Load Lists before returning the Page
        RoomList.AddRange(SelectListHelper.PopulateList(
            await _rooms.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync(),
            e => e.Name,
            bookingId
        ));

        StaffList.AddRange(SelectListHelper.PopulateList(
            await _staff.Where(X => X.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))).ToListAsync(),
            e => e.Name,
            bookingId
        ));
    }
}
