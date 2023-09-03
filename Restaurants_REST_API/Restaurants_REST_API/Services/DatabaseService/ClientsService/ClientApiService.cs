using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;
        private readonly IConfiguration _config;
        private readonly string _newReservationStatus;


        public ClientApiService(MainDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

            _newReservationStatus = _config["ApplicationSettings:ReservationStatus:New"];
            try
            {
                if (string.IsNullOrEmpty(_newReservationStatus))
                {
                    throw new Exception("Reservation status (NEW) can't be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<GetClientDataDTO?> GetClientDetailedDataByIdAsync(int clientId)
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

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsDetailedDataByClientIdAsync(int clientId)
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

        public async Task<GetReservationDTO?> GetReservationDetailedDataByCliennIdReservationIdAsync(int clientId, int reservationId)
        {
            return await
                (from r in _context.Reservation
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

        public async Task<bool> MakeReservationByClientIdAsync(int clientId, ReservationDAO newReservation)
        {
            try
            {
                var newReservationQuery = _context.Add
                (
                    new Reservation
                    {
                        ReservationDate = newReservation.ReservationDate,
                        HowManyPeoples = newReservation.HowManyPeoples,
                        IdClient = clientId,
                        ReservationGrade = null,
                        ReservationStatus = _newReservationStatus,
                        IdRestaurant = newReservation.IdRestaurant
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

        public async Task<bool> UpdateReservationStatusAsync(int clientId, int reservationId, string status)
        {
            try
            {
                var getReservationQuery = await _context.Reservation
                    .Where(r => r.IdReservation == reservationId && r.IdClient == clientId)
                    .FirstAsync();

                getReservationQuery.ReservationStatus = status;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateReservationGradeAsync(int clientId, int reservationId, int grade)
        {
            try
            {
                var getReservationQuery = await _context.Reservation
                    .Where(r => r.IdReservation == reservationId && r.IdClient == clientId)
                    .FirstAsync();

                getReservationQuery.ReservationGrade = grade;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }


        public async Task<bool> MakeComplainByClientIdAsync(int clientId, GetReservationDTO reservationData, GetComplaintDTO complaintData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var restaurantId = await (_context.Reservation
                        .Where(r => r.IdReservation == reservationData.IdReservation && r.IdClient == clientId))
                        .Select(r => new { IdRestaurant = r.IdRestaurant })
                        .FirstAsync();

                    var newComplaintQuery = _context.Add
                    (
                        new Complaint
                        {
                            ComplaintMessage = complaintData.Message,
                            ComplainDate = complaintData.ComplaintDate,
                            ComplaintStatus = complaintData.Status,
                            IdReservation = reservationData.IdReservation,
                            IdRestaurant = restaurantId.IdRestaurant
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
    }
}
