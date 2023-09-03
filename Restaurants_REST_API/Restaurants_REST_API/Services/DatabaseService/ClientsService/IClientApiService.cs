using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public interface IClientApiService
    {
        public Task<GetClientDataDTO?> GetClientDetailedDataByIdAsync(int clientId); 
        public Task<IEnumerable<GetReservationDTO>?> GetAllReservationsDetailedDataByClientIdAsync(int clientId);
        public Task<GetReservationDTO?> GetReservationDetailedDataByCliennIdReservationIdAsync(int clientId, int reservationId);
        public Task<bool> MakeReservationByClientIdAsync(int clientId, ReservationDAO newReservation);
        public Task<bool> UpdateReservationStatusAsync(int clientId, int reservationId, string status);
        public Task<bool> UpdateReservationGradeAsync(int clientId, int reservationId, int grade);
        public Task<bool> MakeComplainByClientIdAsync(int clientId, GetReservationDTO reservation, GetComplaintDTO newComplaint);
    }
}
