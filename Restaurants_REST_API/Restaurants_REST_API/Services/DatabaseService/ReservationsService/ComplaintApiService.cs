using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class ComplaintApiService : IComplaintApiService
    {
        private readonly MainDbContext _context;

        public ComplaintApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetClientDataDTO>?> GetClientComplaintsByStatusAsync(string status)
        {
            return await (from c in _context.Client
                          select new GetClientDataDTO
                          {
                              IdClient = c.IdClient,
                              Name = c.Name,
                              IsBusinessman = c.IsBusinessman,
                              ClientReservations = (from r in _context.Reservation
                                                    where r.IdClient == c.IdClient
                                                    && r.Complaint.ComplaintStatus == status
                                                    select new GetReservationDTO
                                                    {
                                                        IdReservation = r.IdReservation,
                                                        ReservationDate = r.ReservationDate,
                                                        Status = r.ReservationStatus,
                                                        ReservationGrade = r.ReservationGrade,
                                                        HowManyPeoples = r.HowManyPeoples,
                                                        ReservationComplaint = new GetComplaintDTO
                                                        {
                                                            IdComplaint = r.Complaint.IdComplaint,
                                                            ComplaintDate = r.Complaint.ComplainDate,
                                                            Status = r.Complaint.ComplaintStatus,
                                                            Message = r.Complaint.ComplaintMessage
                                                        }

                                                    }).ToList(),

                          }).ToListAsync();
        }

        public Task<bool> UpdateComplaintStatusByComplaintId(int complaint, string status)
        {
            throw new NotImplementedException();
        }
    }
}
