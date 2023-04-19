using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.Database
{
    public class EmployeesInRestaurant
    {
        public int IdRestaurantWorker { get; set; }


        public int IdEmployee { get; set; }
        [ForeignKey(nameof(IdEmployee))]
        public virtual Employee Employee { get; set; }


        public int IdType { get; set; }
        [ForeignKey(nameof(IdType))]
        public virtual EmployeeType EmployeeType { get; set; }


        public int IdRestaurant { get; set; }
        [ForeignKey(nameof(IdRestaurant))]
        public virtual Restaurant Restaurant { get; set; }
    }
}
