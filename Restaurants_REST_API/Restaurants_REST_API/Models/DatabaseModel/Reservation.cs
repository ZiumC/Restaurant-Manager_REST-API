﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.Database
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationStatus { get; set; }
        public int? ReservationGrade { get; set; }
        public int HowManyPeoples { get; set; }


        public int IdClient { get; set; }

        [ForeignKey(nameof(IdClient))]
        public virtual Client Clients { get; set; }


        public int IdRestaurant { get; set; }

        [ForeignKey(nameof(IdRestaurant))]
        public virtual Restaurant Restaurant { get; set; }


        public virtual Complaint Complaint { get; set; }
    }
}
