using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.QueryExtensions;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Staff;

public class EditModel : PageModel
{
    private readonly IRepository<Core.Entities.Staff> _staff;

    [BindProperty]
    public StaffVM StaffVM { get; set; }
    public IList<Booking> Bookings { get; set; } = new List<Booking>();

    public EditModel(IRepository<Core.Entities.Staff> staff)
    {
        _staff = staff;
    }

    public async void OnGetAsync(Guid id)
    {
        var staff = await _staff.GetByIdWithIncludes(id);
        StaffVM = StaffVM.Create(staff);
    }
}
