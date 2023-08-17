using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;


        public ClientApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<GetClientDataDTO?> GetClientDataByIdAsync(int clientId)
        {
            return await _context.Client
                .Where(c => c.IdClient == clientId)
                .Select(c => new GetClientDataDTO
                {
                    IdClient = c.IdClient,
                    Name = c.Name,
                    IsBusinessman = c.IsBusinessman,
                    ClientReservations = null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsDataByClientIdAsync(int clientId)
        {
            return await _context.Reservation
                .Where(r => r.IdClient == clientId)
                .Select(r => new GetReservationDTO
                {
                    IdReservation = r.IdReservation,
                    ReservationDate = r.ReservationDate,
                    Status = r.ReservationStatus,
                    ReservationGrade = r.ReservationGrade,
                    HowManyPeoples = r.HowManyPeoples,
                    ReservationComplaint = _context.Complaint
                                                    .Where(c => c.IdReservation == r.IdReservation)
                                                    .Select(c => new GetComplaintDTO
                                                    {
                                                        IdComplaint = c.IdComplaint,
                                                        ComplaintDate = c.ComplainDate,
                                                        Status = c.ComplaintStatus,
                                                        Message = c.ComplaintMessage
                                                    }).FirstOrDefault()
                }).ToListAsync();
        }

        public async Task<GetReservationDTO?> GetReservationDetailsByCliennIdReservationIdAsync(int clientId, int reservationId)
        {
            return await (from r in _context.Reservation
                          join c in _context.Client
                          on r.IdClient equals c.IdClient
                          where c.IdClient == clientId && r.IdReservation == reservationId
                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,
                              ReservationComplaint = _context.Complaint
                                                    .Where(c => c.IdReservation == r.IdReservation)
                                                    .Select(c => new GetComplaintDTO
                                                    {
                                                        IdComplaint = c.IdComplaint,
                                                        ComplaintDate = c.ComplainDate,
                                                        Status = c.ComplaintStatus,
                                                        Message = c.ComplaintMessage
                                                    }).FirstOrDefault()
                          }).FirstOrDefaultAsync();
        }

        public async Task<bool> CancelReservationByClientIdReservationIdAsync(int clientId, int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmReservationByClientIdReservationIdAsync(int clientId, int reservationId)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> MakeComplaintByClientIdAsync(int clientId, PostComplaintDTO newComplaint)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MakeReservationAsync(PostReservationDTO newReservation)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RateReservationByReservationIdAsync(int reserationId, int reservationGrade)
        {
            throw new NotImplementedException();
        }
    }
}
