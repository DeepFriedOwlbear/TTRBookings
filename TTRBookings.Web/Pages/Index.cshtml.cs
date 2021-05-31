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
        //private readonly ILogger<IndexModel> _logger;
        private readonly IRepository repository;

        public IndexModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {

        }
    }
}
