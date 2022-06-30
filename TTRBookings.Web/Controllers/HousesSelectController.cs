using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Controllers;

[Route("api/[controller]")] //< =  https://localhost:12345/api/bookings
[ApiController]
public class HousesSelectController : ControllerBase
{
    private readonly ILogger<HousesSelectController> _logger;
    private readonly IRepository repository;

    public HousesSelectController(ILogger<HousesSelectController> logger, IRepository repository)
    {
        _logger = logger;
        this.repository = repository;
    }

    [HttpPost]
    [Route("select")]//< =  https://localhost:12345/api/HousesSelect/Select
                     //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
    public IActionResult Select([FromForm] HousesSelectDTO house, [FromForm] string currentPath)
    {
        //Sets the HouseId for the Session when picked through Shared/Components/Houses/Default.cshtml
        HttpContext.Session.SetString("HouseId", house.HouseId);

        //splits the route from the posted Form to the base level. "/Bookings/Edit" gets redirected to "/Bookings" etc.
        return Redirect("/" + currentPath.Split("/")[1]);
    }
}

public sealed class HousesSelectDTO
{
    public string HouseId { get; set; }
}
