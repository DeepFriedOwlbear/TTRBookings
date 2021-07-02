using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (action != "delete")
            {
                CheckAgainstBusinessRules(staffDTO);

                //if business logic threw errors return a failed success state and toastr errors
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

        private void CheckAgainstBusinessRules(StaffDTO staffDTO)
        {
            //Check if form fields are filled in
            if (string.IsNullOrWhiteSpace(staffDTO.StaffName))
            {
                ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
                ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
                return;
            }

            //Assign staffVM values
            StaffVM staffVM = new StaffVM();
            staffVM.Id = staffDTO.StaffId;
            staffVM.Name = staffDTO.StaffName;

            //Load all staff members where the HouseId matches
            IList<Staff> existing = new List<Staff>();
            if (staffVM.Id == Guid.Empty)
            {
                existing = repository.ListWithIncludes<Staff>(
                    staff => !staff.IsDeleted
                    && staff.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                );
            }
            else
            {
                existing = repository.ListWithIncludes<Staff>(
                    staff => !staff.IsDeleted
                    && staff.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                    && staff.Id != staffVM.Id
                );
            }

            if (existing.Any())
            {
                //Is there already a staff member with the same name?
                if (existing.Where(staff => staffVM.Name == staff.Name).Any())
                {
                    ModelState.AddModelError("StaffWithSameName", "[Name]: A staff member with the same name exists already.");
                    ToastrErrors.Add("Name already in use", "There is already a staff member with the same name, names have to be unique.");
                }
            }
        }
    }
}
