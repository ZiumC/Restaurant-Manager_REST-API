using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class ReservationApiService : IReservationApiService
    {
        private readonly MainDbContext _context;

        public ReservationApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsAsync()
        {
            return await (from r in _context.Reservation

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,

                              ReservationComplaint = (from c in _context.Complaint
                                                     where c.IdReservation == r.IdReservation

                                                     select new GetComplaintDTO
                                                     {
                                                         IdComplaint = c.IdComplaint,
                                                         ComplaintDate = c.ComplainDate,
                                                         Status = c.ComplaintStatus,
                                                         Message = c.ComplaintMessage

                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }


        public async Task<IEnumerable<GetReservationDTO>?> GetRestaurantReservationsAsync(int restaurantId)
        {
            return await (from r in _context.Reservation

                          where r.IdRestauration == restaurantId

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,

                              ReservationComplaint = (from c in _context.Complaint
                                                     where c.IdReservation == r.IdReservation

                                                     select new GetComplaintDTO
                                                     {
                                                         IdComplaint = c.IdComplaint,
                                                         ComplaintDate = c.ComplainDate,
                                                         Status = c.ComplaintStatus,
                                                         Message = c.ComplaintMessage
                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }
        public async Task<GetReservationDTO?> GetReservationByIdAsync(int reservationId)
        {
            return await (from r in _context.Reservation
                          where r.IdReservation == reservationId

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,
                              ReservationComplaint = (from c in _context.Complaint
                                                     where c.IdReservation == reservationId

                                                     select new GetComplaintDTO
                                                     {
                                                         IdComplaint = c.IdComplaint,
                                                         ComplaintDate = c.ComplainDate,
                                                         Status = c.ComplaintStatus,
                                                         Message = c.ComplaintMessage
                                                     }
                                                   ).FirstOrDefault()
                          }
                          ).FirstOrDefaultAsync();
        }

        public async Task<GetClientDataDTO?> GetReservationsByClientIdAsync(int clientId)
        {
            return await (from cli in _context.Client
                          where cli.IdClient == clientId

                          select new GetClientDataDTO
                          {
                              IdClient = cli.IdClient,
                              Name = cli.Name,
                              IsBusinessman = cli.IsBusinessman,
                              ClientReservations = (from r in _context.Reservation
                                                    where r.IdClient == clientId

                                                    select new GetReservationDTO
                                                    {
                                                        IdReservation = r.IdReservation,
                                                        ReservationDate = r.ReservationDate,
                                                        Status = r.ReservationStatus,
                                                        ReservationGrade = r.ReservationGrade,
                                                        HowManyPeoples = r.HowManyPeoples,
                                                        ReservationComplaint = (from c in _context.Complaint
                                                                               where c.IdReservation == r.IdReservation

                                                                               select new GetComplaintDTO
                                                                               {
                                                                                   IdComplaint = c.IdComplaint,
                                                                                   ComplaintDate = c.ComplainDate,
                                                                                   Status = c.ComplaintStatus,
                                                                                   Message = c.ComplaintMessage
                                                                               }
                                                                               ).FirstOrDefault()
                                                    }
                                                    ).ToList()
                          }
                          ).FirstOrDefaultAsync();
        }
    }
}
