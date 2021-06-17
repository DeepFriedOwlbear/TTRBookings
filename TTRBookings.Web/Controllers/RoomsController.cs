using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Controllers
{
    [Route("api/[controller]")] //< =  https://localhost:12345/api/bookings
    [ApiController]
    public class RoomsController : ControllerBase
    {   
        private readonly ILogger<RoomsController> _logger;
        private readonly IRepository repository;

        public RoomsController(ILogger<RoomsController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("delete")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Delete(RoomDeleteDTO room)
        {            
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Room>(room.RoomId)) });
        }
    }

    public sealed class RoomDeleteDTO
    {
        public Guid RoomId { get; set; }
    }
}
