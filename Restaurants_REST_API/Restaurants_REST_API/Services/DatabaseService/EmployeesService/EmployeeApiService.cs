﻿using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;
using System.Net;
using System.Threading.Tasks;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly MainDbContext _context;
        private readonly decimal _basicBonus = 150;

        public EmployeeApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {

            return await (from emp in _context.Employees

                          select new EmployeeDTO
                          {
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              Surname = emp.LastName,
                              PESEL = emp.PESEL,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              HiredDate = emp.HiredDate,
                              IsOwner = emp.IsOwner,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,

                              Address = (from addr in _context.Address
                                         where addr.IdAddress == emp.IdAddress

                                         select new AddressDTO
                                         {
                                             IdAddress = addr.IdAddress,
                                             City = addr.City,
                                             Street = addr.Street,
                                             BuildingNumber = addr.BuildingNumber,
                                             LocalNumber = addr.LocalNumber,
                                         }
                                         ).First(),

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
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
                              IdEmployee = employee.IdEmployee,
                              FirstName = employee.FirstName,
                              Surname = employee.LastName,
                              PESEL = employee.PESEL,
                              Salary = employee.Salary,
                              BonusSalary = employee.BonusSalary,
                              HiredDate = employee.HiredDate,
                              IsOwner = employee.IsOwner,
                              FirstPromotionChefDate = employee.FirstPromotionChefDate,

                              Address = new AddressDTO
                              {
                                  IdAddress = address.IdAddress,
                                  City = address.City,
                                  Street = address.Street,
                                  BuildingNumber = address.BuildingNumber,
                                  LocalNumber = address.LocalNumber
                              },

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == employee.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
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
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              Surname = emp.LastName,
                              PESEL = emp.PESEL,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              HiredDate = emp.HiredDate,
                              IsOwner = emp.IsOwner,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,

                              Address = (from addr in _context.Address
                                         where addr.IdAddress == emp.IdAddress

                                         select new AddressDTO
                                         {
                                             IdAddress = addr.IdAddress,
                                             City = addr.City,
                                             Street = addr.Street,
                                             BuildingNumber = addr.BuildingNumber,
                                             LocalNumber = addr.LocalNumber,
                                         }
                                         ).First(),

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
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

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesByRestaurantIdAsync(int restaurantId)
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join emp in _context.Employees
                          on eir.IdEmployee equals emp.IdEmployee

                          join addr in _context.Address
                          on emp.IdAddress equals addr.IdAddress

                          where eir.IdRestaurant == restaurantId

                          select new EmployeeDTO
                          {
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              Surname = emp.LastName,
                              PESEL = emp.PESEL,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              HiredDate = emp.HiredDate,
                              IsOwner = emp.IsOwner,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,

                              Address = (from addr in _context.Address
                                         where addr.IdAddress == emp.IdAddress

                                         select new AddressDTO
                                         {
                                             IdAddress = addr.IdAddress,
                                             City = addr.City,
                                             Street = addr.Street,
                                             BuildingNumber = addr.BuildingNumber,
                                             LocalNumber = addr.LocalNumber,
                                         }
                                         ).First(),

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new CertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }).ToList()
                          }
                          ).ToListAsync();
        }
        public Task<bool> AddNewEmployee(EmployeeDTO newEmployee)
        {
            var bonusSal = newEmployee.BonusSalary;
            if (bonusSal < _basicBonus)
            {
                bonusSal = +_basicBonus;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newAddress = _context.Address.Add
                        (
                            new Address
                            {
                                City = newEmployee.Address.City,
                                Street = newEmployee.Address.Street,
                                BuildingNumber = newEmployee.Address.BuildingNumber,
                                LocalNumber = newEmployee.Address.LocalNumber
                            }
                    );

                    _context.SaveChanges();

                    var newEmp = _context.Add
                        (
                            new Employee
                            {
                                FirstName = newEmployee.FirstName,
                                LastName = newEmployee.Surname,
                                PESEL = newEmployee.PESEL,
                                HiredDate = newEmployee.HiredDate,
                                FirstPromotionChefDate = newEmployee.FirstPromotionChefDate,
                                Salary = newEmployee.Salary,
                                BonusSalary = bonusSal,
                                IsOwner = newEmployee.IsOwner,
                                IdAddress = newAddress.Entity.IdAddress
                            }
                        );


                    _context.SaveChanges();
                    transaction.Commit();
                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Task.FromResult(false);
                }

            }
        }
    }
}
