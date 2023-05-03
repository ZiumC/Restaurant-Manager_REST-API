using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly MainDbContext _context;
        private readonly int _idOwnerType = 1;
        public EmployeeApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesAsync()
        {

            return await (from emp in _context.Employees

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

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
                                              on empCert.IdCertificate equals cert.IdCertificate

                                              where empCert.IdEmployee == emp.IdEmployee

                                              select new GetCertificateDTO
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

        public async Task<GetEmployeeDTO> GetDetailedEmployeeDataAsync(Employee employee)
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

                Certificates = await (from empCert in _context.EmployeeCertificates
                                      join cert in _context.Certificates
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

        public async Task<IEnumerable<int>?> GetSupervisorsIdAsync()
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join et in _context.EmployeeTypes
                          on eir.IdType equals et.IdType

                          where eir.IdType == 2
                          select eir.IdEmployee

                         ).ToListAsync();
        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetSupervisorsDetailsAsync(List<int> supervisorsID)
        {

            return await (from emp in _context.Employees
                          join addr in _context.Address
                          on emp.IdAddress equals addr.IdAddress

                          where supervisorsID.Contains(emp.IdEmployee)

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

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
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

        // Owner employee type should has always id equals 1.
        public async Task<Employee?> GetOwnerBasicDataAsync()
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join et in _context.EmployeeTypes
                          on eir.IdType equals et.IdType

                          join emp in _context.Employees
                          on eir.IdEmployee equals emp.IdEmployee

                          where et.IdType == _idOwnerType

                          select emp
                          ).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesByRestaurantIdAsync(int restaurantId)
        {
            return await (from eir in _context.EmployeesInRestaurants
                          join emp in _context.Employees
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

                              Certificates = (from empCert in _context.EmployeeCertificates
                                              join cert in _context.Certificates
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


        public async Task<IEnumerable<GetEmployeeTypeDTO?>> GetAllTypesAsync()
        {
            return await _context.EmployeeTypes
                .Select(x => new GetEmployeeTypeDTO { IdType = x.IdType, Name = x.Name }).ToListAsync();
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
                                   new EmployeeCertificates
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


        public async Task<bool> AddNewEmployeeTypeAsync(string name)
        {
            try
            {
                var newType = _context.Add
                (
                    new EmployeeType
                    {
                        Name = name
                    }
                );

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateExistingEmployeeByIdAsync(int id, Employee employeeData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateEmployeeQuery = await
                        (_context.Employees
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

        public async Task<bool> UpdateExistingEmployeeCertificatesByIdAsync(List<PutCertificateDTO> updatedCertificatesData, List<int> certificatesId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    for (int i = 0; i < certificatesId.Count(); i++)
                    {
                        var updateCertNameQuery = await
                            (_context.Certificates
                            .Where(c => c.IdCertificate == certificatesId.ElementAt(i))
                            .FirstAsync());

                        updateCertNameQuery.Name = updatedCertificatesData.ElementAt(i).Name;

                        await _context.SaveChangesAsync();

                        var updateCertExpiriationDateQuery = await
                            (_context.EmployeeCertificates
                            .Where(ec => ec.IdCertificate == certificatesId.ElementAt(i))
                            .FirstAsync());

                        updateCertExpiriationDateQuery.ExpirationDate = updatedCertificatesData.ElementAt(i).ExpirationDate;
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

        public async Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateEmpTypeQuery = await
                        (_context.EmployeesInRestaurants
                        .Where(eir => eir.IdEmployee == empId && eir.IdRestaurant == restaurantId)
                        .FirstAsync()
                        );

                    updateEmpTypeQuery.IdType = typeId;

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
    }
}
