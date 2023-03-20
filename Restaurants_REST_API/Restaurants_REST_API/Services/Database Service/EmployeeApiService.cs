using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;
using System.Linq;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly MainDbContext _context;

        public EmployeeApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            return await (from emp in _context.Employees
                    join addr in _context.Address
                    on emp.IdAddress equals addr.IdAddress

                    join empCert in _context.EmployeeCertificates
                    on emp.IdEmployee equals empCert.IdEmployee

                    join cert in _context.Certificates
                    on empCert.IdCertificate equals cert.IdCertificate

                    select new EmployeeDTO
                    {

                        FirstName = emp.Name,
                        Surname = emp.Surname,
                        PESEL = emp.PESEL,
                        Salary = emp.Salary,
                        HiredDate = emp.HiredDate,
                        IsOwner = emp.IsOwner,
                        IsHealthBook = emp.IsHealthBook,
                        City = addr.City,
                        Street = addr.Street,
                        NoBuilding = addr.NoBuilding,
                        NoLocal = addr.NoLocal,
                        CertificateName = cert.Name,
                        ExpirationDate = empCert.ExpirationDate

                    }).ToListAsync();
        }

        public Task<Employee> GetEmployeeByIdAsync(int empId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllSupervisorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetSupervisorByIdAsync(int empId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetOwnerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

    }
}
