using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository<House> _houses;
    public List<House> Houses { get; } = new List<House> { };

    public IndexModel(IRepository<House> houses)
    {
        _houses = houses;
    }

    public async Task OnGetAsync()
    {
        //Fetch the firstOrDefault houseId from the DB
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
        {
            HttpContext.Session.SetString("HouseId", await _houses.Select(x => x.Id.ToString()).FirstOrDefaultAsync());
        }
    }
}
