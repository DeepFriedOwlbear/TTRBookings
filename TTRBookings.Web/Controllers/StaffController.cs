﻿using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")] //< =  https://localhost:12345/api/staff
    [ApiController]
    public class StaffController : ControllerBase
    {   
        private readonly ILogger<StaffController> _logger;
        private readonly IRepository repository;
        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public StaffController(ILogger<StaffController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        //------------------------------------------------------------------------------------------------------------
        //--API Calls-------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        [HttpPost]          //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        [Route("create")]   //< =  https://localhost:12345/api/staff/delete
        public IActionResult Create(StaffDTO staffDTO)
        {
            return HandleStaff(staffDTO, "create");
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(StaffDTO staffDTO)
        {
            return HandleStaff(staffDTO, "edit");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(StaffDTO staffDTO)
        {
            return HandleStaff(staffDTO, "delete");
        }

        //------------------------------------------------------------------------------------------------------------
        //--staff Methods----------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public JsonResult HandleStaff(StaffDTO staffDTO, string action)
        {
            //check if form fields are filled in
            if (action != "delete")
            {
                CheckFormFields(staffDTO);

                //if form fields were filled correctly, check against business logic
                if (ToastrErrors.Count == 0)
                    CheckAgainstBusinessRules(staffDTO);

                //if form fields or business logic threw errors, return a failed success state and toastr errors
                if (ToastrErrors.Count > 0)
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }

            switch (action)
            {
                case "create":
                    return CreateStaff(staffDTO);
                case "edit":
                    return EditStaff(staffDTO);
                case "delete":
                    return DeleteStaff(staffDTO);
                default:
                    ModelState.AddModelError("ErrorOccured", "An error occured while performing this operation.");
                    ToastrErrors.Add("An error occured", "An error occured while performing this operation.");
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }
        }

        public JsonResult CreateStaff(StaffDTO staffDTO)
        {
            //assign staffDTO values to staff
            Staff staff = new Staff(staffDTO.StaffName);
            staff.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            return new JsonResult(new { Success = repository.CreateEntry(staff) });
        }

        public JsonResult EditStaff(StaffDTO staffDTO)
        {
            Staff staff = repository.ReadEntry<Staff>(staffDTO.StaffId);
            staff.Name = staffDTO.StaffName;

            return new JsonResult(new { Success = repository.UpdateEntry(staff) });
        }

        public JsonResult DeleteStaff(StaffDTO staffDTO)
        {
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Staff>(staffDTO.StaffId)) });
        }

        //------------------------------------------------------------------------------------------------------------
        //--Data Transfer Object--------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public sealed class StaffDTO
        {
            public Guid StaffId { get; set; }
            public string StaffName { get; set; }
        }

        //------------------------------------------------------------------------------------------------------------
        //--Business Logic checks-------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void CheckFormFields(StaffDTO staffDTO)
        {
            if (string.IsNullOrWhiteSpace(staffDTO.StaffName))
            {
                ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
                ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            }
        }

        //Checks the "business rules" of a staff
        private void CheckAgainstBusinessRules(StaffDTO staffDTO)
        {
            //Assign staffVM values
            StaffVM staffVM = new StaffVM();
            staffVM.Id = staffDTO.StaffId;
            staffVM.Name = staffDTO.StaffName;

            //TODO - Need to add Business Logic to staffs
        }
    }
}
