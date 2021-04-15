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
    public class ManagersController : ControllerBase
    {   
        private readonly ILogger<ManagersController> _logger;
        private readonly IRepository repository;

        public ManagersController(ILogger<ManagersController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("delete")]//< =  https://localhost:12345/api/bookings/delete
                         //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        public IActionResult Delete(ManagerDeleteDTO manager)
        {            
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Manager>(manager.ManagerId)) });
        }
    }

    public sealed class ManagerDeleteDTO
    {
        public Guid ManagerId { get; set; }
    }
}
