using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models
{
    public class EmployeeCertificates
    {

        public int IdEmployeeCertificate { get; set; }   

        public DateTime ExpirationDate { get; set; }
    

        public int IdEmployee { get; set; }
        [ForeignKey(nameof(IdEmployee))]
        public virtual Employee Employee { get; set; }


        public int IdCertificate { get; set; }
        [ForeignKey(nameof(IdCertificate))]
        public virtual Certificate Certificate { get; set; }

    }
}
