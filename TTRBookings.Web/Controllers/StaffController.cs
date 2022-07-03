using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

[Route("api/[controller]")] //< =  https://localhost:12345/api/staff
[ApiController]
public class StaffController : ControllerBase
{
    private readonly ILogger<StaffController> _logger;
    private readonly IRepository<Staff> _staff;
    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public StaffController(
        ILogger<StaffController> logger, 
        IRepository<Staff> staff)
    {
        _logger = logger;
        _staff=staff;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(StaffDTO staffDTO)
    {
        CheckAgainstBusinessRules(staffDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Staff staff = new Staff(staffDTO.Name)
        {
            HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"))
        };

        return new JsonResult(new { Success = await _staff.AddAsync(staff) });
    }

    public async Task<IActionResult> Edit(StaffDTO staffDTO)
    {
        CheckAgainstBusinessRules(staffDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Staff staff = await _staff.GetByIdAsync(staffDTO.Id);
        staff.Name = staffDTO.Name;

        return new JsonResult(new { Success = await _staff.UpdateAsync(staff) });
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(StaffDTO staffDTO)
    {
        Staff staff = await _staff.GetByIdAsync(staffDTO.Id);

        return new JsonResult(new { Success = await _staff.DeleteAsync(staff) });
    }

    private void CheckAgainstBusinessRules(StaffDTO staffDTO)
    {
        //Check if form fields are filled in
        if (string.IsNullOrWhiteSpace(staffDTO.Name))
        {
            ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
            ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            return;
        }

        // Load all staff where the HouseId matches
        var existingQuery = _staff.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                                           && x.IsArchived == false);

        // If the Id is not empty, select all staff that aren't the provided room
        if (staffDTO.Id != Guid.Empty)
            existingQuery.Where(x => x.Id != staffDTO.Id);

        // Is there already a staff with the same name?
        if (existingQuery.Where(x => x.Name == staffDTO.Name).Any())
        {
            ModelState.AddModelError("StaffWithSameName", "[Name]: A staff member with the same name exists already.");
            ToastrErrors.Add("Name already in use", "There is already a staff member with the same name, names have to be unique.");
        }
    }
}
