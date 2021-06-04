using System;
using System.Linq;
using TTRBookings.Core.Entities;
using Xunit;

namespace CoreUnitTests
{
    public class ManagerUnitTests
    {
        [Fact]
        public void CreateManager_ShouldAddToManagers_GivenCreateManagerCalled()
        {
            House house = new House("House_1");
            Manager manager1 = house.CreateManager("Alice");

            Assert.Equal(manager1, house.Managers.First());
        }
    }

    public class BookingUnitTests
    {
        [Fact]
        public void CreateBooking_ShouldBeValidBooking_GivenSensibleParameters()
        {
            var timeSlot = new TimeSlot(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2));
            Booking booking = Booking.Create(Guid.NewGuid(), new Rose(null), new Tier(), new Room(null), timeSlot);
            Assert.False(booking.IsDeleted);
            Assert.NotEqual(Guid.Empty, booking.HouseId);
            Assert.Equal(booking.TimeSlot, timeSlot);
        }
    }

    public class TimeSlotUnitTests
    {
        [Theory]
        [InlineData(1)]
        public void CreateTimeslot_ShouldThrow_GivenStartAfterEnd(int hourOffset)
        {
            Assert.Throws<BookingStartTimeAfterEndTime>(() => new TimeSlot(DateTime.Now.AddHours(hourOffset), DateTime.Now));
        }

        [Fact]
        public void CreateTimeslot_ShouldThrow_GivenStartInThePast()
        {
            Assert.Throws<BookingStartTimeInThePast>(() => new TimeSlot(DateTime.Now.AddHours(-1), DateTime.Now));
        }

        [Fact]
        public void CreateTimeslot_ShouldThrow_GivenDurationMoreThan24Hours()
        {
            Assert.Throws<BookingDurationLongerThan24Hours>(() => new TimeSlot(DateTime.Now.AddHours(1), DateTime.Now.AddHours(25)));
        }
    }
}
