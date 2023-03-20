using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Restaurants_REST_API.Models
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }

        [Required]
        [Column(TypeName="money")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime HiredDate { get; set; }

        [Required]
        [MaxLength(1)]
        public string IsHealthBook { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PESEL { get; set; }

        
        public virtual int IdAddress { get; set; }

        [ForeignKey(nameof(IdAddress))]
        public virtual Address Address { get; set; }

    }
}
