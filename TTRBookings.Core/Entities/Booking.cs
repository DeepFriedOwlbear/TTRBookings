using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace TTRBookings.Core.Entities
{
    public class Booking : BaseEntity
    {
        public Guid HouseId { get; private set; }
        public Rose Rose { get; private set; }
        public Room Room { get; private set; }
        public Tier Tier { get; private set; }
        public TimeSlot TimeSlot { get; private set; }

        private Booking() { } //needed by EntityFramework

        private Booking(Rose rose, Tier tier, Room room, TimeSlot timeSlot)
        {
            Rose = rose ?? throw new ArgumentNullException(nameof(rose));
            Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            TimeSlot = timeSlot ?? throw new ArgumentNullException(nameof(timeSlot));
        }

        public static Booking Create(Guid houseId, Rose rose, Tier tier, Room room, TimeSlot timeslot)
        {
            var booking = new Booking(rose, tier, room, timeslot);
            booking.HouseId = houseId;
            return booking;
        }

        //public void Edit<TEntity>(params TEntity[] edits)
        //    where TEntity : BaseEntity
        //{
        //    foreach(TEntity entity in edits)
        //    {
        //        switch (entity)
        //        {
        //            case Room x:
        //                Room = x;
        //                break;
        //            case Rose x:
        //                Rose = x;
        //                break;
        //            case Tier x:
        //                Tier = x;
        //                break;
        //            case TimeSlot x:
        //                TimeSlot = x;
        //                break;
        //            default: break;
        //        }
        //    }
        //}

        public void Edit<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {            
            switch (entity)
            {
                case Room x:
                    Room = x;
                    break;
                case Rose x:
                    Rose = x;
                    break;
                case Tier x:
                    Tier = x;
                    break;
                case TimeSlot x:
                    TimeSlot = x;
                    break;
                default: break;
            }
        }
    }
}