using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Staff
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public IList<Core.Entities.Staff> Staff { get; set; }

        //public IndexModel(ILogger<IndexModel> logger, IRepository repository)
        //{
        //    _logger = logger;
        //    this.repository = repository;
        //}

        public IndexModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            Staff = repository.List<Core.Entities.Staff>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")));
        }
    }
}
