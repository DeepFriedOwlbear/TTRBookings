using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Managers
{
    public class CreateModel : PageModel
    {
        private readonly IRepository repository;

        [BindProperty] public ManagerVM ManagerVM { get; set; }

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }
    }
}
