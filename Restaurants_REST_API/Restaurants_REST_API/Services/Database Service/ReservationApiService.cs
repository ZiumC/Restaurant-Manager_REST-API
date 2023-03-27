using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class ReservationApiService : IReservationApiService
    {
        private readonly MainDbContext _context;

        public ReservationApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReservationDTO>?> GetAllReservationsAsync()
        {
            return await (from r in _context.Reservations

                          select new ReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              TableNumber = r.TableNumber,

                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == r.IdReservation

                                                     select new ComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus
                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }


        public Task<IEnumerable<ReservationDTO>?> GetReservationsByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }
        public Task<ComplainDTO> GetComplainsByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }
    }
}
