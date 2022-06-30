using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Staff;

public class EditModel : PageModel
{
    private readonly IRepository repository;

    [BindProperty]
    public StaffVM StaffVM { get; set; }
    public IList<Booking> Bookings { get; set; } = new List<Booking>();

    public EditModel(IRepository repository)
    {
        this.repository = repository;
    }

    public void OnGet(Guid id)
    {
        var staff = repository.ReadEntryWithIncludes<Core.Entities.Staff>(
            id,
            _ => _.Tiers);
        StaffVM = StaffVM.Create(staff);
    }
}
