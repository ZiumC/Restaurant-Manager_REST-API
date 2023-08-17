using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public interface IClientApiService
    {
        public Task<GetClientDataDTO?> GetClientDataByIdAsync(int clientId); 
        public Task<IEnumerable<GetReservationDTO>?> GetAllReservationsDataByClientIdAsync(int clientId);
        public Task<GetReservationDTO?> GetReservationDetailsByCliennIdReservationIdAsync(int clientId, int reservationId);
        public Task<bool> MakeReservationAsync(PostReservationDTO newReservation);
        public Task<bool> ConfirmReservationByClientIdReservationIdAsync(int clientId, int reservationId);
        public Task<bool> CancelReservationByClientIdReservationIdAsync(int clientId, int reservationId);
        public Task<bool> MakeComplaintByClientIdAsync(int clientId, PostComplaintDTO newComplaint);
        public Task<bool> RateReservationByReservationIdAsync(int reserationId, int reservationGrade);
    }
}
