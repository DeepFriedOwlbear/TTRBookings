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
    public class RosesController : ControllerBase
    {   
        private readonly ILogger<RosesController> _logger;
        private readonly IRepository repository;

        public RosesController(ILogger<RosesController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("delete")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Delete(RoseDeleteDTO rose)
        {            
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Rose>(rose.RoseId)) });
        }
    }

    public sealed class RoseDeleteDTO
    {
        public Guid RoseId { get; set; }
    }
}
