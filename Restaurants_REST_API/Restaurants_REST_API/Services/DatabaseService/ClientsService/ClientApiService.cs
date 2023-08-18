﻿using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;
        private readonly IConfiguration _configuration;


        public ClientApiService(MainDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public async Task<bool> MakeReservationByClientIdAsync(int clientId, PostReservationDTO newReservation)
        {
            try
            {
                var newReservatin = _context.Add
                (
                    new Reservation
                    {
                        ReservationDate = newReservation.ReservationDate,
                        HowManyPeoples = newReservation.HowManyPeoples,
                        IdClient = clientId,
                        ReservationGrade = null,
                        ReservationStatus = _configuration["ApplicationSettings:ReservationStatus:New"],
                        IdRestauration = newReservation.IdRestaurant
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

        public async Task<bool> UpdateReservationStatusByClientIdAsync(int clientId, GetReservationDTO reservation)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateReservation = await (_context.Reservation
                        .Where(r => r.IdReservation == reservation.IdReservation && r.IdClient == clientId))
                        .FirstAsync();

                    updateReservation.ReservationStatus = reservation.Status;

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


        public async Task<bool> MakeComplainByClientIdAsync(int clientId, GetReservationDTO reservationData, GetComplaintDTO complaint)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var restaurantId = await (_context.Reservation
                        .Where(r => r.IdReservation == reservationData.IdReservation && r.IdClient == clientId))
                        .Select(r => new { IdRestaurant = r.IdRestauration })
                        .FirstAsync();

                    var newComplaint = _context.Add
                    (
                        new Complaint
                        {
                            ComplaintMessage = complaint.Message,
                            ComplainDate = complaint.ComplaintDate,
                            ComplaintStatus = complaint.Status,
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


        public async Task<bool> RateReservationByReservationIdAsync(int reserationId, int reservationGrade)
        {
            throw new NotImplementedException();
        }
    }
}
