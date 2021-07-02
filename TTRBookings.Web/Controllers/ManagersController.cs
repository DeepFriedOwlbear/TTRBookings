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
    [Route("api/[controller]")] //< =  https://localhost:12345/api/managers
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly ILogger<ManagersController> _logger;
        private readonly IRepository repository;
        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public ManagersController(ILogger<ManagersController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        //------------------------------------------------------------------------------------------------------------
        //--API Calls-------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        [HttpPost]          //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        [Route("create")]   //< =  https://localhost:12345/api/managers/delete
        public IActionResult Create(ManagerDTO managerDTO)
        {
            return HandleManager(managerDTO, "create");
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(ManagerDTO managerDTO)
        {
            return HandleManager(managerDTO, "edit");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(ManagerDTO managerDTO)
        {
            return HandleManager(managerDTO, "delete");
        }

        //------------------------------------------------------------------------------------------------------------
        //--Manager Methods-------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public JsonResult HandleManager(ManagerDTO managerDTO, string action)
        {
            if (action != "delete")
            {
                CheckAgainstBusinessRules(managerDTO);

                //if business logic threw errors return a failed success state and toastr errors
                if (ToastrErrors.Count > 0)
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }

            switch (action)
            {
                case "create":
                    return CreateManager(managerDTO);
                case "edit":
                    return EditManager(managerDTO);
                case "delete":
                    return DeleteManager(managerDTO);
                default:
                    ModelState.AddModelError("ErrorOccured", "An error occured while performing this operation.");
                    ToastrErrors.Add("An error occured", "An error occured while performing this operation.");
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }
        }

        public JsonResult CreateManager(ManagerDTO managerDTO)
        {
            //assign managerDTO values to manager
            Manager manager = new Manager(managerDTO.ManagerName);
            manager.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            return new JsonResult(new { Success = repository.CreateEntry(manager) });
        }
        
        public JsonResult EditManager(ManagerDTO managerDTO)
        {
            Manager manager = repository.ReadEntry<Manager>(managerDTO.ManagerId);
            manager.Name = managerDTO.ManagerName;

            return new JsonResult(new { Success = repository.UpdateEntry(manager) });
        }

        public JsonResult DeleteManager(ManagerDTO managerDTO)
        {
            return new JsonResult(new { Success = repository.DeleteEntry(repository.ReadEntry<Manager>(managerDTO.ManagerId)) });
        }

        //------------------------------------------------------------------------------------------------------------
        //--Data Transfer Object--------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public sealed class ManagerDTO
        {
            public Guid ManagerId { get; set; }
            public string ManagerName { get; set; }
        }

        //------------------------------------------------------------------------------------------------------------
        //--Business Logic checks-------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void CheckAgainstBusinessRules(ManagerDTO managerDTO)
        {
            if (string.IsNullOrWhiteSpace(managerDTO.ManagerName))
            {
                ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
                ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
                return;
            }

            //Assign ManagerVM values
            ManagerVM managerVM = new ManagerVM();
            managerVM.Id = managerDTO.ManagerId;
            managerVM.Name = managerDTO.ManagerName;

            //TODO - Need to add Business Logic to Managers
        }
    }
}
