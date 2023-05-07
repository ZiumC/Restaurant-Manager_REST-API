namespace Restaurants_REST_API.Models.Database
{
    public class EmployeeType
    {
        public int IdType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeRestaurant> EmployeeTypes { get; set; }
    }
}
