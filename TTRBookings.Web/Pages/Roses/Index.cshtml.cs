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

namespace TTRBookings.Web.Pages.Roses
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public IList<Rose> Roses { get; set; }

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
            Roses = repository.List<Rose>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId")));
        }
    }
}
