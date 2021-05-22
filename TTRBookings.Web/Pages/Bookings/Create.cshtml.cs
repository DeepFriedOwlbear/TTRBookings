using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        //private readonly ILogger<CreateModel> _logger;
        private readonly IRepository repository;

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> RoseList { get; } = new List<SelectListItem> { };

        [BindProperty] public BookingVM BookingVM { get; set; }

        //public CreateModel(ILogger<CreateModel> logger, IRepository repository)
        //{
        //    _logger = logger;
        //    this.repository = repository;
        //}

        public CreateModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {
            RoomList.AddRange(SelectListHelper.PopulateList<Room>(
                repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name
            ));

            RoseList.AddRange(SelectListHelper.PopulateList<Rose>(
                repository.List<Rose>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name
            ));
        }

        public IActionResult OnPost()
        {
            //place backend validation logic here.
            CheckAgainstBusinessRules();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Tier tier = new Tier()
            {
                Unit = BookingVM.Tier.Unit,
                Rate = BookingVM.Tier.Rate
            };

            Booking booking = Booking.Create(
                repository.ReadEntry<Rose>(BookingVM.Rose.Id),
                tier,
                repository.ReadEntry<Room>(BookingVM.Room.Id),
                new TimeSlot(BookingVM.TimeSlot.Start, BookingVM.TimeSlot.End)
            );

            booking.HouseId = Guid.Parse(HttpContext.Session.GetString("HouseId"));

            //store in database
            repository.CreateEntry(booking);

            //return/redirect user to somewhere
            return Redirect("/Bookings/");
        }


        private void CheckAgainstBusinessRules()
        {
            if (BookingVM.TimeSlot.Start < DateTime.Now)
            {
                ModelState.AddModelError("StartDateBeforeCurrentDate", "[StartDate]: Cannot be in the past.");
            }
            if (BookingVM.TimeSlot.Start > BookingVM.TimeSlot.End)
            {
                ModelState.AddModelError("EndDateBeforeStartDate", "[EndDate]: Cannot be before [StartDate]");
            }

            //Check Database for bookings with Rose & Room that happen at or between TimeSlot.Start and TimeSlot.End
            //Why check rose?
            //Why check room?
            //Why check both start and end?

            IList<Booking> existing = repository.ListWithIncludes<Booking>(
                //the filter
                booking => !booking.IsDeleted
                && booking.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                && booking.Room.Id == BookingVM.Room.Id
                && booking.Rose.Id == BookingVM.Rose.Id
                ,
                //the includes
                _ => _.Room, _ => _.Rose, _ => _.TimeSlot);

            if (existing.Any()) // input data from database, to check against input from frontend
            {
                //Is Start time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    BookingVM.TimeSlot.Start > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.Start < booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotStartContainedWithinExistingTimeslot", "[StartDate]: Cannot be within timeslot of existing booking.");
                }

                //Is End time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    BookingVM.TimeSlot.End > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.End < booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotEndContainedWithinExistingTimeslot", "[EndDate]: Cannot be within timeslot of existing booking.");
                }

                //Is Start time & End Time of EXISTING booking contained within timeslot of NEW booking?
                if (existing.Where(booking =>
                    booking.TimeSlot.Start > BookingVM.TimeSlot.Start
                    && booking.TimeSlot.Start < BookingVM.TimeSlot.End
                    && booking.TimeSlot.End > BookingVM.TimeSlot.Start
                    && booking.TimeSlot.End < BookingVM.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("ExistingTimeslotStartAndEndContainedWithinNewTimeslot", "[ExistingStartDate] and [ExistingEndDate]: Cannot be within timeslot of new booking.");
                }

                //Is Start time & End Time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    BookingVM.TimeSlot.Start > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.Start < booking.TimeSlot.End
                    && BookingVM.TimeSlot.End > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.End < booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotStartAndEndContainedWithinExistingTimeslot", "[StartDate] and [EndDate]: Cannot be within timeslot of existing booking.");
                    ModelState.Remove("NewTimeslotStartContainedWithinExistingTimeslot");
                    ModelState.Remove("NewTimeslotEndContainedWithinExistingTimeslot");
                }

                //Is Start time of NEW booking within timeslot of EXISTING booking X, and End time of NEW booking within timeslot of EXISTING booking Y?
                var condition1 = existing.Where(booking =>
                    BookingVM.TimeSlot.Start > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.Start < booking.TimeSlot.End);
                if (condition1.Any())
                {
                    Guid idCondition1 = condition1.FirstOrDefault().Id;
                    var condition2 = existing.Where(booking =>
                    BookingVM.TimeSlot.End > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.End < booking.TimeSlot.End 
                    && booking.Id != idCondition1).Any();

                    if (condition2)//check case X of overlap schema.
                    {
                        ModelState.AddModelError("NewBookingOverlapsWithinMultipleExistingTimeslots", "[StartDate and EndDate]: Cannot overlap with existing bookings.");
                    }
                }
            }
        }
    }
}
