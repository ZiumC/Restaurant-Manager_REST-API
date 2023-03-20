﻿namespace Restaurants_REST_API.Models
{
    public class EmployeeType
    {
        public int IdType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeesInRestaurant> EmployeeTypes { get; set;}
    }
}
