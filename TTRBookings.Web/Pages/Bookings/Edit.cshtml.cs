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
using TTRBookings.Core.QueryExtensions;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings;

public class EditModel : PageModel
{
    private readonly IRepository<Room> _rooms;
    private readonly IRepository<Core.Entities.Staff> _staff;
    private readonly IRepository<Booking> _bookings;

    [BindProperty]
    public BookingVM BookingVM { get; set; }

    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public List<SelectListItem> RoomList { get; } = new List<SelectListItem>();
    public List<SelectListItem> StaffList { get; } = new List<SelectListItem>();

    public EditModel(
        IRepository<Room> rooms,
        IRepository<Core.Entities.Staff> staff,
        IRepository<Booking> bookings)
    {
        _rooms=rooms;
        _staff=staff;
        _bookings=bookings;
    }

    public async Task OnGetAsync(Guid id)
    {
        //Load booking matching Id
        var booking = await _bookings.GetByIdWithIncludes(id);
        //Populate drop-down lists
        await PopulateLists(booking.Id);
        //convert booking to bookingvm here;
        BookingVM = BookingVM.Create(booking);
    }

    private async Task PopulateLists(Guid bookingId)
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
