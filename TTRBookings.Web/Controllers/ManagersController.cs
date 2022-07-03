using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.DTOs;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagersController : ControllerBase
{
    private readonly ILogger<ManagersController> _logger;
    private readonly IRepository<Manager> _managers;

    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public ManagersController(
        ILogger<ManagersController> logger, 
        IRepository<Manager> managers)
    {
        _logger = logger;
        _managers = managers;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(ManagerDTO managerDTO)
    {
        CheckAgainstBusinessRules(managerDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Manager manager = new Manager(managerDTO.Name)
        {
            HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"))
        };

        return new JsonResult(
            new { 
                Success = await _managers.AddAsync(manager) 
            });
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit(ManagerDTO managerDTO)
    {
        CheckAgainstBusinessRules(managerDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Manager manager = await _managers.GetByIdAsync(managerDTO.Id);
        manager.Name = managerDTO.Name;

        return new JsonResult(
            new { 
                Success = await _managers.UpdateAsync(manager) 
            });
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(ManagerDTO managerDTO)
    {
        Manager manager = await _managers.GetByIdAsync(managerDTO.Id);

        return new JsonResult(
            new {
                Success = await _managers.DeleteAsync(manager)
            });
    }

    private void CheckAgainstBusinessRules(ManagerDTO managerDTO)
    {
        // Check if form fields are filled in
        if (string.IsNullOrWhiteSpace(managerDTO.Name))
        {
            ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
            ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            return;
        }

        // Load all managers where the HouseId matches
        var existingQuery = _managers.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                                              && x.IsArchived == false);

        // If the Id is not empty, select all managers that aren't the provided Manager
        if (managerDTO.Id != Guid.Empty)
            existingQuery.Where(x => x.Id != managerDTO.Id);

        // Is there already a manager with the same name?
        if (existingQuery.Where(x => x.Name == managerDTO.Name).Any())
        {
            ModelState.AddModelError("ManagerWithSameName", "[Name]: A manager with the same name exists already.");
            ToastrErrors.Add("Name already in use", "There is already a manager with the same name, names have to be unique.");
        }
    }
}
