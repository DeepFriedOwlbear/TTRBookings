using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Staff;

public class IndexModel : PageModel
{
    private readonly IRepository repository;

    public IList<Core.Entities.Staff> Staff { get; set; }

    public IndexModel(IRepository repository)
    {
        this.repository = repository;
    }

    public void OnGet()
    {
        Staff = repository.List<Core.Entities.Staff>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")));
    }
}
