﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.Database
{
    public class Complaint
    {
        public int IdComplaint { get; set; }
        public DateTime ComplainDate { get; set; }
        public string ComplaintStatus { get; set; }
        public string ComplaintMessage { get; set; }


        public int IdRestaurant { get; set; }
        [ForeignKey(nameof(IdRestaurant))]
        public virtual Restaurant Restaurant { get; set; }


        public int IdReservation { get; set; }
        [ForeignKey(nameof(IdReservation))]
        public virtual Reservation Reservation { get; set; }
    }
}
