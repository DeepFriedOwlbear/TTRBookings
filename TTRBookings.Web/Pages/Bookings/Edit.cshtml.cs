using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Helpers;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly IRepository repository;
        
        [BindProperty]
        public BookingVM BookingVM { get; set; }

        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem>();
        public List<SelectListItem> StaffList { get; } = new List<SelectListItem>();

        public EditModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            //Load booking matching Id
            var booking = repository.ReadEntryWithIncludes<Booking>(id, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);

            //Populate drop-down lists
            PopulateLists(booking.Id);

            //convert booking to bookingvm here;
            BookingVM = BookingVM.Create(booking);
        }

        public IActionResult OnPost()
        {
            //Back-End validation
            CheckAgainstBusinessRules();

            //if ModelState is invalid, populate drop-down lists and reload the page
            if (!ModelState.IsValid)
            {
                PopulateLists(BookingVM.Id);
                return Page();
            }

            //retrieve original from database.
            Booking booking = repository.ReadEntryWithIncludes<Booking>(BookingVM.Id, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);
            House house = repository.ReadEntryWithIncludes<House>(booking.HouseId, _ => _.Managers, _ => _.Rooms, _ => _.Staff, _ => _.Bookings, _ => _.Bookings[0].Tier, _ => _.Bookings[0].TimeSlot);

            //assign bookingVM values to booking
            house.UpdateBooking(
                booking,
                new Tier(BookingVM.Tier.Rate),
                repository.ReadEntry<Core.Entities.Staff>(BookingVM.Staff.Id),
                repository.ReadEntry<Room>(BookingVM.Room.Id),
                new TimeSlot(BookingVM.TimeSlot.Start, BookingVM.TimeSlot.End));

            //store in database
            repository.UpdateEntry(house);

            //return/redirect user
            return RedirectToPage("/Bookings/Edit", new { booking.Id });
        }

        private void PopulateLists(Guid bookingId)
        {
            RoomList.AddRange(SelectListHelper.PopulateList(
                repository.List<Room>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));

            StaffList.AddRange(SelectListHelper.PopulateList(
                repository.List<Core.Entities.Staff>(_ => _.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))),
                e => e.Name,
                bookingId
            ));
        }

        private void CheckAgainstBusinessRules()
        {
            //Bookings can't be made in the past.
            if (BookingVM.TimeSlot.Start < DateTime.Now)
            {
                ModelState.AddModelError("StartDateBeforeCurrentDate", "[StartDate]: Cannot be in the past.");
                ToastrErrors.Add("Invalid Start Date", "Start Date can't be in the past.");
            }
            //TimeSlot Start can't be after TimeSlot End
            if (BookingVM.TimeSlot.Start > BookingVM.TimeSlot.End)
            {
                ModelState.AddModelError("EndDateBeforeStartDate", "[EndDate]: Cannot be before [StartDate]");
                ToastrErrors.Add("Invalid End Date", "End Date can't be before Start Date.");
            }
            //Duration between TimeSlot Start and TimeSlot End can't be longer than 24 hours
            if ((BookingVM.TimeSlot.End - BookingVM.TimeSlot.Start).TotalHours >= 24)
            {
                ModelState.AddModelError("BookingDurationLongerThan24Hours", "[EndDate]: Duration between [StartDate] and [EndDate] cannot be longer than 24 hours.");
                ToastrErrors.Add("Invalid Booking Duration", "Booking duration can't be longer than 24 hours.");
            }

            //TODO - existing has wrong list of bookings if Staff & Room changed during booking. Makes it possible to have overlapping timeslot.
            //Load all bookings where the HouseId, RoomId and StaffId matches
            IList<Booking> existing = repository.ListWithIncludes<Booking>(
                //the filter
                booking => !booking.IsDeleted
                && booking.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                && booking.Room.Id == BookingVM.Room.Id
                && booking.Staff.Id == BookingVM.Staff.Id
                && booking.Id != BookingVM.Id
                ,
                //the includes
                _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);

            if (existing.Any()) // input data from database, to check against input from frontend
            {
                //Is Start time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    BookingVM.TimeSlot.Start > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.Start < booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotStartContainedWithinExistingTimeslot", "[StartDate]: Cannot be within timeslot of existing booking.");
                    ToastrErrors.Add("Start Time Scheduling Issue", "The Start Time overlaps with an existing booking.");
                }

                //Is End time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    BookingVM.TimeSlot.End > booking.TimeSlot.Start
                    && BookingVM.TimeSlot.End < booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotEndContainedWithinExistingTimeslot", "[EndDate]: Cannot be within timeslot of existing booking.");
                    ToastrErrors.Add("End Time Scheduling Issue", "The End Time overlaps with an existing booking.");
                }

                //Is Start time & End Time of EXISTING booking contained within timeslot of NEW booking?
                if (existing.Where(booking =>
                    booking.TimeSlot.Start > BookingVM.TimeSlot.Start
                    && booking.TimeSlot.Start < BookingVM.TimeSlot.End
                    && booking.TimeSlot.End > BookingVM.TimeSlot.Start
                    && booking.TimeSlot.End < BookingVM.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("ExistingTimeslotStartAndEndContainedWithinNewTimeslot", "[ExistingStartDate] and [ExistingEndDate]: Cannot be within timeslot of new booking.");
                    ToastrErrors.Add("Existing Booking Overlap", "A booking that uses that timeslot already exists.");
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

                    ToastrErrors.Add("New Booking Overlap", "The new booking overlaps an existing booking.");
                    ToastrErrors.Remove("Start Time Scheduling Issue");
                    ToastrErrors.Remove("End Time Scheduling Issue");
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
                        ToastrErrors.Add("New Booking Overlaps Multiple Bookings", "The new booking overlaps at least two existing booking.");
                    }
                }
            }
        }
    }
}
