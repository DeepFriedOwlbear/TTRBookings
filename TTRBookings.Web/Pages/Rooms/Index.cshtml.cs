using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Rooms;

public class IndexModel : PageModel
{
    private readonly IRepository repository;

    public IList<Room> Rooms { get; set; }

    public IndexModel(IRepository repository)
    {
        this.repository = repository;
    }

    public void OnGet()
    {
        Rooms = repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")));
    }
}
