using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Helpers;

namespace TTRBookings.Web.ViewComponents;

public class HousesViewComponent : ViewComponent
{
    public List<SelectListItem> Houses { get; } = new List<SelectListItem> { };

    private readonly IRepository<House> _houses;
    public HousesViewComponent(IRepository<House> houses)
    {
        _houses=houses;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
        {
            Houses.AddRange(SelectListHelper.PopulateList(
                await _houses.GetAllAsync(), 
                x => x.Name
                ));

            HttpContext.Session.SetString("HouseId", Houses.FirstOrDefault().Value);
        }
        else
        {
            Houses.AddRange(SelectListHelper.PopulateList(
                await _houses.GetAllAsync(), 
                x => x.Name,
                Guid.Parse(HttpContext.Session.GetString("HouseId"))
                ));
        }

        return View(new SelectedHouse(HttpContext.Session.GetString("HouseId"), Houses));
    }
}

public class SelectedHouse
{
    public List<SelectListItem> Houses { get; } = new List<SelectListItem> { };
    public string Id { get; set; }

    public SelectedHouse(string houseId, List<SelectListItem> houses)
    {
        Id = houseId;
        Houses = houses;
    }
}
