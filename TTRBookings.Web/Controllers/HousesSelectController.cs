using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.DTOs;

namespace TTRBookings.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HousesSelectController : ControllerBase
{
    private readonly ILogger<HousesSelectController> _logger;

    public HousesSelectController(ILogger<HousesSelectController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("select")]
    public IActionResult Select([FromForm] HousesSelectDTO house, [FromForm] string currentPath)
    {
        //Sets the HouseId for the Session when picked through Shared/Components/Houses/Default.cshtml
        HttpContext.Session.SetString("HouseId", house.Id);

        //splits the route from the posted Form to the base level. "/Bookings/Edit" gets redirected to "/Bookings" etc.
        return Redirect("/" + currentPath.Split("/")[1]);
    }
}