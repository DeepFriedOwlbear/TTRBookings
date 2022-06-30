using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Managers;

public class IndexModel : PageModel
{
    private readonly IRepository repository;

    public IList<Manager> Managers { get; set; }

    public IndexModel(IRepository repository)
    {
        this.repository = repository;
    }

    public void OnGet()
    {
        Managers = repository.List<Manager>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")));
    }
}
