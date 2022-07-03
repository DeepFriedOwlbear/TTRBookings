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

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly ILogger<RoomsController> _logger;
    private readonly IRepository<Room> _rooms;

    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public RoomsController(
        ILogger<RoomsController> logger, 
        IRepository<Room> rooms)
    {
        _logger = logger;
        _rooms=rooms;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(RoomDTO roomDTO)
    {
        CheckAgainstBusinessRules(roomDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Room room = new Room(roomDTO.Name)
        {
            HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"))
        };

        return new JsonResult(new { Success = await _rooms.AddAsync(room) });
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit(RoomDTO roomDTO)
    {
        CheckAgainstBusinessRules(roomDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Room room = await _rooms.GetByIdAsync(roomDTO.Id);
        room.Name = roomDTO.Name;

        return new JsonResult(new { Success = await _rooms.UpdateAsync(room) });
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(RoomDTO roomDTO)
    {
        Room room = await _rooms.GetByIdAsync(roomDTO.Id);

        return new JsonResult(new { Success = await _rooms.DeleteAsync(room) });
    }

    private void CheckAgainstBusinessRules(RoomDTO roomDTO)
    {
        //Check if form fields are filled in
        if (string.IsNullOrWhiteSpace(roomDTO.Name))
        {
            ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
            ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            return;
        }

        // Load all rooms where the HouseId matches
        var existingQuery = _rooms.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                                           && x.IsArchived == false);

        // If the Id is not empty, select all rooms that aren't the provided room
        if (roomDTO.Id != Guid.Empty)
            existingQuery.Where(x => x.Id != roomDTO.Id);

        // Is there already a room with the same name?
        if (existingQuery.Where(x => x.Name == roomDTO.Name).Any())
        {
            ModelState.AddModelError("RoomWithSameName", "[Name]: A room with the same name exists already.");
            ToastrErrors.Add("Name already in use", "There is already a room with the same name, names have to be unique.");
        }
    }
}
