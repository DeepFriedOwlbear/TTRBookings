using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Controllers
{
    [Route("api/[controller]")] //< =  https://localhost:12345/api/rooms
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger<RoomsController> _logger;
        private readonly IRepository repository;
        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public RoomsController(ILogger<RoomsController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        //------------------------------------------------------------------------------------------------------------
        //--API Calls-------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        [HttpPost]          //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        [Route("create")]   //< =  https://localhost:12345/api/rooms/delete
        public IActionResult Create(RoomDTO roomDTO)
        {
            return HandleRoom(roomDTO, "create");
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(RoomDTO roomDTO)
        {
            return HandleRoom(roomDTO, "edit");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(RoomDTO roomDTO)
        {
            return HandleRoom(roomDTO, "delete");
            //return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Room>(roomDTO.RoomId)) });
        }

        //------------------------------------------------------------------------------------------------------------
        //--Room Methods----------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public JsonResult HandleRoom(RoomDTO roomDTO, string action)
        {
            //check if form fields are filled in
            if (action != "delete")
            {
                CheckFormFields(roomDTO);

                //if form fields were filled correctly, check against business logic
                if (ToastrErrors.Count == 0)
                    CheckAgainstBusinessRules(roomDTO);

                //if form fields or business logic threw errors, return a failed success state and toastr errors
                if (ToastrErrors.Count > 0)
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }

            switch (action)
            {
                case "create":
                    return CreateRoom(roomDTO);
                case "edit":
                    return EditRoom(roomDTO);
                case "delete":
                    return DeleteRoom(roomDTO);
                default:
                    ModelState.AddModelError("ErrorOccured", "An error occured while performing this operation.");
                    ToastrErrors.Add("An error occured", "An error occured while performing this operation.");
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }
        }

        public JsonResult CreateRoom(RoomDTO roomDTO)
        {
            //assign roomDTO values to room
            Room room = new Room(roomDTO.RoomName);
            room.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            return new JsonResult(new { Success = repository.CreateEntry(room) });
        }

        public JsonResult EditRoom(RoomDTO roomDTO)
        {
            Room room = repository.ReadEntry<Room>(roomDTO.RoomId);
            room.Name = roomDTO.RoomName;

            return new JsonResult(new { Success = repository.UpdateEntry(room) });
        }

        public JsonResult DeleteRoom(RoomDTO roomDTO)
        {
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Room>(roomDTO.RoomId)) });
        }

        //------------------------------------------------------------------------------------------------------------
        //--Data Transfer Object--------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public sealed class RoomDTO
        {
            public Guid RoomId { get; set; }
            public string RoomName { get; set; }
        }

        //------------------------------------------------------------------------------------------------------------
        //--Business Logic checks-------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void CheckFormFields(RoomDTO roomDTO)
        {
            if (string.IsNullOrWhiteSpace(roomDTO.RoomName))
            {
                ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
                ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            }
        }

        //Checks the "business rules" of a room
        private void CheckAgainstBusinessRules(RoomDTO roomDTO)
        {
            //Assign RoomVM values
            RoomVM roomVM = new RoomVM();
            roomVM.Id = roomDTO.RoomId;
            roomVM.Name = roomDTO.RoomName;

            //TODO - Need to add Business Logic to Rooms
        }
    }
}
