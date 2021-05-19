using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Controllers
{
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
        [Route("select")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Select([FromForm] HousesSelectDTO house, [FromForm] string currentPath)
        {
            //TODO - Session HouseId gets set, but only displays a json result now.
            //Can't use JavaScript in ViewComponent, so have to reroute to the page that the request was made from.
            //@Context.Request.Path is sent in the Form from "Shared/Components/Houses/Default.cshtml" via CurrentPath
            //need to trim the @Context.Request.Path.
            //For example: "/", "/Bookings", "/Roses" is alright; "/Bookings/Edit", "/Roses/Details" needs to be trimmed to redirect to "/Bookings" or "/Roses" respectively.

            HttpContext.Session.SetString("HouseId", house.HouseId);
            return Redirect(currentPath);
        }
    }

    public sealed class HousesSelectDTO
    {
        public string HouseId { get; set; }
    }
}
