using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Bookings
{
    //https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/da1?view=aspnetcore-5.0
    
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public IList<Booking> Bookings { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public void OnGet()
        {
            Bookings = repository.ListWithIncludes<Booking>(_ => true, _ => _.Room);
        }
    }
}
