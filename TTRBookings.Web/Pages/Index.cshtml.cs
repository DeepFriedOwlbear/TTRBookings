using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages
{

    public class IndexModel : PageModel
    {
        private readonly IRepository repository;
        public List<House> Houses { get; } = new List<House> { };

        public Guid StaffId { get; set; }

        public IndexModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            //Fetch the firstOrDefault houseId from the DB
            HttpContext.Session.SetString("HouseId", repository.List<House>().FirstOrDefault().Id.ToString());
        }
    }
}
