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


                              certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }
                                              ).ToList()

                          }).ToListAsync();

        }

        public async Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId)
        {
            return await _context.Employees
                .Where(e => e.IdEmployee == empId)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeDTO> GetDetailedEmployeeDataAsync(Employee employee)
        {

            Address address = await _context.Address
                .Where(a => a.IdAddress == employee.IdAddress)
                .FirstAsync();

            return await (from empCert in _context.EmployeeCertificates
                          join cert in _context.Certificates
                          on empCert.IdCertificate equals cert.IdCertificate

                          where empCert.IdEmployee == employee.IdEmployee
                          select new EmployeeDTO
                          {
                              FirstName = employee.Name,
                              Surname = employee.Surname,
                              PESEL = employee.PESEL,
                              Salary = employee.Salary,
                              HiredDate = employee.HiredDate,
                              IsOwner = employee.IsOwner,
                              IsHealthBook = employee.IsHealthBook,
                              City = address.City,
                              Street = address.Street,
                              NoBuilding = address.NoBuilding,
                              NoLocal = address.NoLocal,

                              certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == employee.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }
                                              ).ToList()

                          }).FirstAsync();
        }
        public async Task<IEnumerable<int>?> GetSupervisorsIdAsync()
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join et in _context.EmployeeTypes
                          on eir.IdType equals et.IdType

                          where eir.IdType == 2
                          select eir.IdEmployee

                         ).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDTO>> GetSupervisorsDetailsAsync(List<int> supervisorsID)
        {

            return await (from emp in _context.Employees
                          join addr in _context.Address
                          on emp.IdAddress equals addr.IdAddress

                          where supervisorsID.Contains(emp.IdEmployee)

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

                              certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }).ToList()

                          }).ToListAsync();

        }

        // Owner employee type should has always id equals 1.
        public async Task<Employee?> GetOwnerBasicDataAsync()
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join et in _context.EmployeeTypes
                          on eir.IdType equals et.IdType

                          join emp in _context.Employees
                          on eir.IdEmployee equals emp.IdEmployee

                          where et.IdType == 1

                          select emp
                          ).FirstOrDefaultAsync();

        }

        public Task<IEnumerable<Employee>> GetAllEmployeesByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

    }
}
