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
            return await (from r in _context.Reservations

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,

                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == r.IdReservation

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage

                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }


        public async Task<IEnumerable<GetReservationDTO>?> GetRestaurantReservationsAsync(int restaurantId)
        {
            return await (from r in _context.Reservations

                          where r.IdRestauration == restaurantId

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,

                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == r.IdReservation

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }
        public async Task<GetReservationDTO?> GetReservationByIdAsync(int reservationId)
        {
            return await (from r in _context.Reservations
                          where r.IdReservation == reservationId

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,
                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == reservationId

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }
                                                   ).FirstOrDefault()
                          }
                          ).FirstOrDefaultAsync();
        }

        public async Task<GetClientDTO?> GetReservationsByClientIdAsync(int clientId)
        {
            return await (from cli in _context.Clients
                          where cli.IdClient == clientId

                          select new GetClientDTO
                          {
                              IdClient = cli.IdClient,
                              Name = cli.Name,
                              IsBusinessman = cli.IsBusinessman,
                              ClientReservations = (from r in _context.Reservations
                                                    where r.IdClient == clientId

                                                    select new GetReservationDTO
                                                    {
                                                        IdReservation = r.IdReservation,
                                                        ReservationDate = r.ReservationDate,
                                                        Status = r.ReservationStatus,
                                                        ReservationGrade = r.ReservationGrade,
                                                        HowManyPeoples = r.HowManyPeoples,
                                                        ReservationComplain = (from c in _context.Complains
                                                                               where c.IdReservation == r.IdReservation

                                                                               select new GetComplainDTO
                                                                               {
                                                                                   IdComplain = c.IdComplain,
                                                                                   ComplainDate = c.ComplainDate,
                                                                                   Status = c.ComplainStatus,
                                                                                   Message = c.ComplainMessage
                                                                               }
                                                                               ).FirstOrDefault()
                                                    }
                                                    ).ToList()
                          }
                          ).FirstOrDefaultAsync();
        }
    }
}
