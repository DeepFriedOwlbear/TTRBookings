using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;

        [BindProperty]
        public BookingVM BookingVM { get; set; }

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> RoseList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> TierList { get; } = new List<SelectListItem> { };

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            PopulateList<Room>();
            PopulateList<Rose>();
            PopulateList<TierRate>();
        }

        //TODO: make PopulateList() prettier, can probably be made sleeker.
        private void PopulateList<TEntity>()
        {
            if (typeof(TEntity) == typeof(Room))
            {
                foreach (Room room in repository.List<Room>())
                {
                    RoomList.Add(new SelectListItem
                    {
                        Value = room.Id.ToString(),
                        Text = room.Name
                    });
                }
            }
            else if (typeof(TEntity) == typeof(Rose))
            {
                foreach (Rose rose in repository.List<Rose>())
                {
                    RoseList.Add(new SelectListItem
                    {
                        Value = rose.Id.ToString(),
                        Text = rose.Name
                    });
                }
            }
            else if (typeof(TEntity) == typeof(TierRate))
            {
                foreach (TierRate tierrate in repository.List<TierRate>())
                {
                    TierList.Add(new SelectListItem
                    {
                        Value = tierrate.Id.ToString(),
                        Text = tierrate.Value.ToString()
                    });
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //new TimeSlot(new DateTime(2021, 1, 4, 23, 00, 00, DateTimeKind.Utc), new DateTime(2021, 1, 5, 00, 30, 00, DateTimeKind.Utc));

            Booking booking = Booking.Create(repository.ReadEntry<Rose>(BookingVM.Rose.Id), repository.ReadEntry<Tier>(BookingVM.Tier.Id), repository.ReadEntry<Room>(BookingVM.Room.Id), new TimeSlot(BookingVM.TimeSlot.Start, BookingVM.TimeSlot.End));
            
            //assign bookingVM values to booking
            //booking.Room = repository.ReadEntry<Room>(BookingVM.Room.Id);
            //booking.Rose = repository.ReadEntry<Rose>(BookingVM.Rose.Id);
            //booking.Tier.Rate = BookingVM.Tier.Rate;
            //booking.TimeSlot.Start = BookingVM.TimeSlot.Start;
            //booking.TimeSlot.End = BookingVM.TimeSlot.End;

            //store in database

            //return/redirect user to somewhere
            return RedirectToPage("/Bookings/Create");
        }
    }
}
