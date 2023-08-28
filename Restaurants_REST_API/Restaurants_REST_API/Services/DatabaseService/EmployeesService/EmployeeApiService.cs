using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.MapperService;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly MainDbContext _context;
        private readonly IConfiguration _config;
        public EmployeeApiService(MainDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesAsync()
        {

            return await (from emp in _context.Employee

                          select new GetEmployeeDTO
                          {
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              LastName = emp.LastName,
                              PESEL = emp.PESEL,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              HiredDate = emp.HiredDate,
                              IsOwner = emp.IsOwner,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,

                              Address = (from addr in _context.Address
                                         where addr.IdAddress == emp.IdAddress

                                         select new GetAddressDTO
                                         {
                                             IdAddress = addr.IdAddress,
                                             City = addr.City,
                                             Street = addr.Street,
                                             BuildingNumber = addr.BuildingNumber,
                                             LocalNumber = addr.LocalNumber,
                                         }).First(),

                              Certificates = (from empCert in _context.EmployeeCertificate
                                              join cert in _context.Certificate
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new GetCertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }).ToList()

                          }).ToListAsync();

        }

        public async Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId)
        {
            return await _context.Employee
                .Where(e => e.IdEmployee == empId)
                .FirstOrDefaultAsync();
        }

        public async Task<GetEmployeeDTO> GetEmployeeDetailsAsync(Employee employee)
        {

            Address address = await _context.Address
                .Where(a => a.IdAddress == employee.IdAddress)
                .FirstAsync();

            return new GetEmployeeDTO
            {
                IdEmployee = employee.IdEmployee,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PESEL = employee.PESEL,
                Salary = employee.Salary,
                BonusSalary = employee.BonusSalary,
                HiredDate = employee.HiredDate,
                FirstPromotionChefDate = employee.FirstPromotionChefDate,
                IsOwner = employee.IsOwner,
                Address = new GetAddressDTO
                {
                    IdAddress = address.IdAddress,
                    City = address.City,
                    Street = address.Street,
                    BuildingNumber = address.BuildingNumber,
                    LocalNumber = address.LocalNumber
                },

                Certificates = await (from empCert in _context.EmployeeCertificate
                                      join cert in _context.Certificate
                                      on empCert.IdCertificate equals cert.IdCertificate

                                      where empCert.IdEmployee == employee.IdEmployee

                                      select new GetCertificateDTO
                                      {
                                          IdCertificate = cert.IdCertificate,
                                          Name = cert.Name,
                                          ExpirationDate = empCert.ExpirationDate

                                      }).ToListAsync()

            };
        }

        public async Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesDetailsByTypeIdAsync(int typeId)
        {
            IEnumerable<GetEmployeeDTO> employeesQuery = await
                (from emp in _context.Employee
                 join eir in _context.EmployeeRestaurant
                 on emp.IdEmployee equals eir.IdEmployee

                 join addr in _context.Address
                 on emp.IdAddress equals addr.IdAddress

                 where eir.IdType == typeId

                 select new GetEmployeeDTO
                 {
                     IdEmployee = emp.IdEmployee,
                     FirstName = emp.FirstName,
                     LastName = emp.LastName,
                     PESEL = emp.PESEL,
                     Salary = emp.Salary,
                     BonusSalary = emp.BonusSalary,
                     HiredDate = emp.HiredDate,
                     IsOwner = emp.IsOwner,
                     FirstPromotionChefDate = emp.FirstPromotionChefDate,

                     Address = new GetAddressDTO
                     {
                         IdAddress = addr.IdAddress,
                         City = addr.City,
                         Street = addr.Street,
                         BuildingNumber = addr.BuildingNumber,
                         LocalNumber = addr.LocalNumber,
                     },

                     Certificates = (from empCert in _context.EmployeeCertificate
                                     join cert in _context.Certificate
                                     on empCert.IdCertificate equals cert.IdCertificate

                                     where empCert.IdEmployee == emp.IdEmployee

                                     select new GetCertificateDTO
                                     {
                                         IdCertificate = cert.IdCertificate,
                                         Name = cert.Name,
                                         ExpirationDate = empCert.ExpirationDate
                                     }).ToList()

                 }).ToListAsync();

            return employeesQuery;
        }

        public async Task<GetEmployeeDTO?> GetEmployeeDetailsByTypeIdAsync(int typeId)
        {
            GetEmployeeDTO? employeeQuery = await
                (from emp in _context.Employee
                 join eir in _context.EmployeeRestaurant
                 on emp.IdEmployee equals eir.IdEmployee

                 join addr in _context.Address
                 on emp.IdAddress equals addr.IdAddress

                 where eir.IdType == typeId

                 select new GetEmployeeDTO
                 {
                     IdEmployee = emp.IdEmployee,
                     FirstName = emp.FirstName,
                     LastName = emp.LastName,
                     PESEL = emp.PESEL,
                     HiredDate = emp.HiredDate,
                     FirstPromotionChefDate = emp.FirstPromotionChefDate,
                     Salary = emp.Salary,
                     BonusSalary = emp.BonusSalary,
                     IsOwner = emp.IsOwner,
                     Address = new GetAddressDTO
                     {
                         IdAddress = addr.IdAddress,
                         City = addr.City,
                         Street = addr.Street,
                         BuildingNumber = addr.BuildingNumber,
                         LocalNumber = addr.LocalNumber,
                     },
                     Certificates = (from empCert in _context.EmployeeCertificate
                                     join cert in _context.Certificate
                                     on empCert.IdCertificate equals cert.IdCertificate

                                     where empCert.IdEmployee == emp.IdEmployee

                                     select new GetCertificateDTO
                                     {
                                         IdCertificate = cert.IdCertificate,
                                         Name = cert.Name,
                                         ExpirationDate = empCert.ExpirationDate
                                     }).ToList()
                 }).FirstOrDefaultAsync();

            return employeeQuery;
        }

        public async Task<Employee?> GetEmployeeDataByPeselAsync(string pesel)
        {
            return await (from emp in _context.Employee

                          where emp.PESEL == pesel

                          select new Employee
                          {
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              LastName = emp.LastName,
                              PESEL = emp.PESEL,
                              HiredDate = emp.HiredDate,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              IsOwner = emp.IsOwner,
                              EmployeeInRestaurant = (from eir in _context.EmployeeRestaurant
                                                      where eir.IdEmployee == emp.IdEmployee
                                                      select new EmployeeRestaurant
                                                      {
                                                          IdType = eir.IdType
                                                      }).ToList()
                          }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetEmployeeDetailsByRestaurantIdAsync(int restaurantId)
        {
            return await (from eir in _context.EmployeeRestaurant
                          join emp in _context.Employee
                          on eir.IdEmployee equals emp.IdEmployee

                          join addr in _context.Address
                          on emp.IdAddress equals addr.IdAddress

                          where eir.IdRestaurant == restaurantId

                          select new GetEmployeeDTO
                          {
                              IdEmployee = emp.IdEmployee,
                              FirstName = emp.FirstName,
                              LastName = emp.LastName,
                              PESEL = emp.PESEL,
                              Salary = emp.Salary,
                              BonusSalary = emp.BonusSalary,
                              HiredDate = emp.HiredDate,
                              IsOwner = emp.IsOwner,
                              FirstPromotionChefDate = emp.FirstPromotionChefDate,

                              Address = (from addr in _context.Address
                                         where addr.IdAddress == emp.IdAddress

                                         select new GetAddressDTO
                                         {
                                             IdAddress = addr.IdAddress,
                                             City = addr.City,
                                             Street = addr.Street,
                                             BuildingNumber = addr.BuildingNumber,
                                             LocalNumber = addr.LocalNumber,
                                         }
                                         ).First(),

                              Certificates = (from empCert in _context.EmployeeCertificate
                                              join cert in _context.Certificate
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new GetCertificateDTO
                                              {
                                                  IdCertificate = cert.IdCertificate,
                                                  Name = cert.Name,
                                                  ExpirationDate = empCert.ExpirationDate
                                              }).ToList()
                          }
                          ).ToListAsync();
        }


        public async Task<bool> AddNewEmployeeAsync(PostEmployeeDTO newEmployee, bool certificatesExist)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newDatabaseAddress = _context.Address.Add
                        (
                            new Address
                            {
                                City = newEmployee.Address.City,
                                Street = newEmployee.Address.Street,
                                BuildingNumber = newEmployee.Address.BuildingNumber,
                                LocalNumber = newEmployee.Address.LocalNumber
                            }
                    );
                    await _context.SaveChangesAsync();

                    var newDatabaseEmployee = _context.Add
                        (
                            new Employee
                            {
                                FirstName = newEmployee.FirstName,
                                LastName = newEmployee.LastName,
                                PESEL = newEmployee.PESEL,
                                HiredDate = DateTime.Now,
                                Salary = newEmployee.Salary,
                                BonusSalary = newEmployee.BonusSalary,
                                IsOwner = "N",
                                IdAddress = newDatabaseAddress.Entity.IdAddress
                            }
                        );
                    await _context.SaveChangesAsync();

                    if (certificatesExist)
                    {
                        //inside EmployeesController certificates are checked if they are NOT NULL and are correct
                        foreach (PostCertificateDTO empCertificate in newEmployee.Certificates)
                        {
                            var newDatabaseCertificate = _context.Add
                                (
                                    new Certificate
                                    {
                                        Name = empCertificate.Name
                                    }
                                );
                            await _context.SaveChangesAsync();

                            var newDatabaseEmpCertificate = _context.Add
                                (
                                   new EmployeeCertificate
                                   {
                                       IdCertificate = newDatabaseCertificate.Entity.IdCertificate,
                                       IdEmployee = newDatabaseEmployee.Entity.IdEmployee,
                                       ExpirationDate = empCertificate.ExpirationDate
                                   }
                                );
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> AddNewEmployeeCertificatesAsync(int empId, IEnumerable<PostCertificateDTO> certificatesData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (PostCertificateDTO postCert in certificatesData)
                    {
                        var newCertificate = _context.Add
                        (
                            new Certificate
                            {
                                Name = postCert.Name
                            }
                        );

                        await _context.SaveChangesAsync();

                        var newEmployeeCertificate = _context.Add
                            (
                                new EmployeeCertificate
                                {
                                    IdEmployee = empId,
                                    ExpirationDate = postCert.ExpirationDate,
                                    IdCertificate = newCertificate.Entity.IdCertificate
                                }
                            );

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> UpdateEmployeeDataByIdAsync(int id, Employee employeeData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateEmployeeQuery = await
                        (_context.Employee
                        .Where(e => e.IdEmployee == id)
                        .FirstAsync());

                    updateEmployeeQuery.FirstName = employeeData.FirstName;
                    updateEmployeeQuery.LastName = employeeData.LastName;
                    updateEmployeeQuery.PESEL = employeeData.PESEL;
                    updateEmployeeQuery.Salary = employeeData.Salary;
                    updateEmployeeQuery.BonusSalary = employeeData.BonusSalary;

                    updateEmployeeQuery.Address.City = employeeData.Address.City;
                    updateEmployeeQuery.Address.Street = employeeData.Address.Street;
                    updateEmployeeQuery.Address.BuildingNumber = employeeData.Address.BuildingNumber;
                    updateEmployeeQuery.Address.LocalNumber = employeeData.Address.LocalNumber;

                    updateEmployeeQuery.HiredDate = updateEmployeeQuery.HiredDate;
                    updateEmployeeQuery.FirstPromotionChefDate = updateEmployeeQuery.FirstPromotionChefDate;
                    updateEmployeeQuery.IsOwner = updateEmployeeQuery.IsOwner;

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }
                await transaction.CommitAsync();
                return true;
            }

        }

        public async Task<bool> UpdateEmployeeCertificatesByIdAsync(int certificateId, PutCertificateDTO updatedCertificatesData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateCertNameQuery = await
                            (_context.Certificate
                            .Where(c => c.IdCertificate == certificateId)
                            .FirstAsync());

                    updateCertNameQuery.Name = updatedCertificatesData.Name;
                    await _context.SaveChangesAsync();

                    var updateCertExpiriationDateQuery = await
                           (_context.EmployeeCertificate
                           .Where(ec => ec.IdCertificate == certificateId)
                           .FirstAsync());

                    updateCertExpiriationDateQuery.ExpirationDate = updatedCertificatesData.ExpirationDate;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> DeleteEmployeeDataByIdAsync(int empId, GetEmployeeDTO employeeData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //removing user if employee has registered in db
                    var userQuery = await _context.User.Where(u => u.IdEmployee == empId).FirstOrDefaultAsync();
                    if (userQuery != null)
                    {
                        _context.Remove(userQuery);
                    }
                    await _context.SaveChangesAsync();

                    var employeeAddressQuery = await
                       (_context.Address.Where(a => a.IdAddress == employeeData.Address.IdAddress)).FirstAsync();

                    _context.Remove(employeeAddressQuery);
                    await _context.SaveChangesAsync();

                    var workerQuery = await
                       (_context.EmployeeRestaurant.Where(eir => eir.IdEmployee == empId)).ToListAsync();

                    foreach (EmployeeRestaurant worker in workerQuery)
                    {
                        var singleWorkerQuery = await
                            (_context.EmployeeRestaurant.Where(eir => eir.IdEmployee == empId)).FirstAsync();

                        _context.Remove(singleWorkerQuery);
                        await _context.SaveChangesAsync();
                    }


                    if (employeeData.Certificates != null && employeeData.Certificates.Count() > 0)
                    {
                        foreach (GetCertificateDTO empCert in employeeData.Certificates)
                        {
                            var certificateQuery = await
                                (_context.Certificate.Where(c => c.IdCertificate == empCert.IdCertificate)).FirstAsync();

                            //removing each employee certificate
                            _context.Remove(certificateQuery);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> DeleteEmployeeCertificateAsync(int empId, GetCertificateDTO employeeCertificateData)
        {
            try
            {
                Certificate certificateToDelete = new Certificate
                {
                    IdCertificate = employeeCertificateData.IdCertificate,
                    Name = employeeCertificateData.Name
                };

                _context.Remove(certificateToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
