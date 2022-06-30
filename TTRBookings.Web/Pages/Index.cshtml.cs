using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository repository;
    public List<House> Houses { get; } = new List<House> { };

    public IndexModel(IRepository repository)
    {
        this.repository = repository;
    }

    public void OnGet()
    {
        //Fetch the firstOrDefault houseId from the DB
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
        {
            HttpContext.Session.SetString("HouseId", repository.List<House>().FirstOrDefault().Id.ToString());
        }
    }
}
