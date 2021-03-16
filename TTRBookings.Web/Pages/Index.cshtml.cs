using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Helpers;

namespace TTRBookings.Web.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        [BindProperty] public string HouseId { get; set; }

        public List<SelectListItem> Houses { get; } = new List<SelectListItem> { };

        public IndexModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                Houses.AddRange(SelectListHelper.PopulateList(
                    repository.List<House>(), e => e.Id.ToString("D")
                    ));

                HttpContext.Session.SetString("HouseId", Houses.FirstOrDefault().Value);
            }
            else
            {
                Houses.AddRange(SelectListHelper.PopulateList(
                    repository.List<House>(), e => e.Id.ToString("D"), 
                    Guid.Parse(HttpContext.Session.GetString("HouseId"))
                    ));
            }
        }

        public IActionResult OnPost()
        {
            Houses.AddRange(SelectListHelper.PopulateList(
                repository.List<House>(), e => e.Id.ToString("D"),
                Guid.Parse(HouseId)
                ));

            HttpContext.Session.SetString("HouseId", HouseId);

            return Page();
        }

    }
}
