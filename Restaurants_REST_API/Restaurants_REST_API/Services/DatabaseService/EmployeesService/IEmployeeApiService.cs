﻿using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IEmployeeApiService
    {
        public Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesAsync();
        public Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId);
        public Task<GetEmployeeDTO> GetDetailedEmployeeDataAsync(Employee employeeData);
        public Task<IEnumerable<GetEmployeeDTO>?> GetAllSupervisorsAsync();
        public Task<Employee?> GetBasicSupervisorDataByIdAsync(int supervisorId);
        public Task<Employee?> GetBasicOwnerDataAsync();
        public Task<IEnumerable<GetEmployeeDTO>> GetDetailedEmployeeDataByRestaurantIdAsync(int restaurantId);
        public Task<bool> AddNewEmployeeAsync(PostEmployeeDTO newEmployee, bool certificatesExist);
        public Task<bool> AddNewEmployeeCertificatesAsync(int empId, IEnumerable<PostCertificateDTO> certificatesData);
        public Task<bool> UpdateEmployeeDataByIdAsync(int empId, Employee updatedEmployeeData);
        public Task<bool> UpdateEmployeeCertificatesByIdAsync(int certificateId, PutCertificateDTO updatedCertificatesData);
        public Task<bool> DeleteEmployeeDataByIdAsync(int empId, GetEmployeeDTO employeeData);
        public Task<bool> DeleteEmployeeCertificateAsync(int empId, GetCertificateDTO employeeCertificateData);

    }
}
