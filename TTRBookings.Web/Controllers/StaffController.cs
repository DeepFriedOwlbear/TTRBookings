using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Controllers
{
    [Route("api/[controller]")] //< =  https://localhost:12345/api/bookings
    [ApiController]
    public class StaffController : ControllerBase
    {   
        private readonly ILogger<StaffController> _logger;
        private readonly IRepository repository;

        public StaffController(ILogger<StaffController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("delete")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Delete(StaffDeleteDTO staff)
        {            
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Staff>(staff.StaffId)) });
        }
    }

    public sealed class StaffDeleteDTO
    {
        public Guid StaffId { get; set; }
    }
}
