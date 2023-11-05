using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly MainDbContext _context;
        public EmployeeApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesAsync()
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

        public async Task<Employee?> GetEmployeeSimpleDataByIdAsync(int empId)
        {
            return await _context.Employee
                .Where(e => e.IdEmployee == empId)
                .FirstOrDefaultAsync();
        }

        public async Task<GetEmployeeDTO?> GetEmployeeDetailsByEmpIdAsync(int empId)
        {

            var getEmpQuery =
                await _context.Employee
                .Where(e => e.IdEmployee == empId)
                .FirstOrDefaultAsync();

            if (getEmpQuery == null)
            {
                return null;
            }

            Address address = await _context.Address
                .Where(a => a.IdAddress == getEmpQuery.IdAddress)
                .FirstAsync();

            return new GetEmployeeDTO
            {
                IdEmployee = getEmpQuery.IdEmployee,
                FirstName = getEmpQuery.FirstName,
                LastName = getEmpQuery.LastName,
                PESEL = getEmpQuery.PESEL,
                Salary = getEmpQuery.Salary,
                BonusSalary = getEmpQuery.BonusSalary,
                HiredDate = getEmpQuery.HiredDate,
                FirstPromotionChefDate = getEmpQuery.FirstPromotionChefDate,
                IsOwner = getEmpQuery.IsOwner,
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

                                      where empCert.IdEmployee == empId

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
            return await
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
        }

        public async Task<GetEmployeeDTO?> GetEmployeeDetailsByTypeIdAsync(int typeId)
        {
            return await
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
        }

        public async Task<Employee?> GetEmployeeSimpleDataByPeselAsync(string pesel)
        {
            return await
                (from emp in _context.Employee

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

        public async Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesDetailsByRestaurantIdAsync(int restaurantId)
        {
            return await
                (from eir in _context.EmployeeRestaurant
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


        public async Task<bool> AddNewEmployeeAsync(EmployeeDAO newEmpData, string ownerStatus)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newAddressQuery = _context.Address.Add
                        (
                            new Address
                            {
                                City = newEmpData.Address.City,
                                Street = newEmpData.Address.Street,
                                BuildingNumber = newEmpData.Address.BuildingNumber,
                                LocalNumber = newEmpData.Address.LocalNumber
                            }
                    );
                    await _context.SaveChangesAsync();

                    var newEmpQuery = _context.Add
                        (
                            new Employee
                            {
                                FirstName = newEmpData.FirstName,
                                LastName = newEmpData.LastName,
                                PESEL = newEmpData.PESEL,
                                HiredDate = DateTime.Now,
                                Salary = newEmpData.Salary,
                                BonusSalary = newEmpData.BonusSalary,
                                IsOwner = ownerStatus,
                                IdAddress = newAddressQuery.Entity.IdAddress
                            }
                        );
                    await _context.SaveChangesAsync();

                    if (newEmpData.Certificates != null && newEmpData.Certificates.Count() > 0)
                    {
                        //inside EmployeesController certificates names are checked if they are NOT NULL
                        foreach (var empCertificate in newEmpData.Certificates)
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
                                       IdEmployee = newEmpQuery.Entity.IdEmployee,
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

        public async Task<bool> AddNewEmployeeCertificatesAsync(int empId, CertificateDAO empCertificatesData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newCertificateQuery = _context.Add
                    (
                        new Certificate
                        {
                            Name = empCertificatesData.Name
                        }
                    );

                    await _context.SaveChangesAsync();

                    var newEmpCertificateQuery = _context.Add
                        (
                            new EmployeeCertificate
                            {
                                IdEmployee = empId,
                                ExpirationDate = empCertificatesData.ExpirationDate,
                                IdCertificate = newCertificateQuery.Entity.IdCertificate
                            }
                        );

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

        public async Task<bool> UpdateEmployeeDataByIdAsync(int id, EmployeeDAO empData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var getEmpQuery = await
                        (_context.Employee
                        .Where(e => e.IdEmployee == id)
                        .FirstAsync());

                    getEmpQuery.FirstName = empData.FirstName;
                    getEmpQuery.LastName = empData.LastName;
                    getEmpQuery.PESEL = empData.PESEL;
                    getEmpQuery.Salary = empData.Salary;
                    getEmpQuery.BonusSalary = empData.BonusSalary;

                    var getEmpAddressQuery =
                        await _context.Address
                        .Where(a => a.IdAddress == getEmpQuery.IdAddress)
                        .FirstAsync();

                    getEmpAddressQuery.City = empData.Address.City;
                    getEmpAddressQuery.Street = empData.Address.Street;
                    getEmpAddressQuery.BuildingNumber = empData.Address.BuildingNumber;
                    getEmpAddressQuery.LocalNumber = empData.Address.LocalNumber;

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

        public async Task<bool> UpdateEmployeeCertificateByIdAsync(int certificateId, CertificateDAO empCertificateData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var getEmpCertificateQuery = await
                            (_context.Certificate
                            .Where(c => c.IdCertificate == certificateId)
                            .FirstAsync());

                    getEmpCertificateQuery.Name = empCertificateData.Name;
                    await _context.SaveChangesAsync();

                    var getEmpCertificatesQuery = await
                           (_context.EmployeeCertificate
                           .Where(ec => ec.IdCertificate == certificateId)
                           .FirstAsync());

                    getEmpCertificatesQuery.ExpirationDate = empCertificateData.ExpirationDate;
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

        public async Task<bool> DeleteEmployeeDataByIdAsync(int empId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //removing user if employee has registered in db
                    var getUserQuery = await _context.User
                        .Where(u => u.IdEmployee == empId)
                        .FirstOrDefaultAsync();
                    if (getUserQuery != null)
                    {
                        _context.Remove(getUserQuery);
                        await _context.SaveChangesAsync();
                    }

                    var getEmpCertificatesIdQuery = await _context.EmployeeCertificate
                        .Where(ec => ec.IdEmployee == empId)
                        .Select(ec => ec.IdCertificate)
                        .ToListAsync();

                    if (getEmpCertificatesIdQuery != null && getEmpCertificatesIdQuery.Count() > 0)
                    {
                        foreach (int empCertificateId in getEmpCertificatesIdQuery)
                        {
                            var empCertificateQuery = await _context.Certificate
                                .Where(c => c.IdCertificate == empCertificateId)
                                .FirstAsync();

                            //removing each employee certificate
                            _context.Remove(empCertificateQuery);
                            await _context.SaveChangesAsync();
                        }
                    }

                    var gerWorkerQuery = await _context.EmployeeRestaurant
                        .Where(eir => eir.IdEmployee == empId)
                        .ToListAsync();
                    foreach (EmployeeRestaurant worker in gerWorkerQuery)
                    {
                        _context.Remove(worker);
                        await _context.SaveChangesAsync();
                    }

                    var getEmpQuery = await _context.Employee
                        .Where(e => e.IdEmployee == empId)
                        .FirstAsync();

                    var getEmpAddressQuery = await _context.Address
                        .Where(a => a.IdAddress == getEmpQuery.IdAddress)
                        .FirstAsync();

                    _context.Remove(getEmpAddressQuery);
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

        public async Task<bool> DeleteEmployeeCertificateAsync(int certificateId, int employeId)
        {
            try
            {
                var getEmployeeCertificateQuery = await _context.EmployeeCertificate
                    .Where(ec => ec.IdCertificate == certificateId && ec.IdEmployee == employeId)
                    .FirstAsync();

                _context.Remove(getEmployeeCertificateQuery);
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
