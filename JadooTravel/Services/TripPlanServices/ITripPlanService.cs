using AutoMapper;
using JadooTravel.Dtos.TripPlanDtos;
using JadooTravel.Entities;
using MongoDB.Driver;

namespace JadooTravel.Services.TripPlanServices
{
    public interface ITripPlanService
    {
        Task<List<ResultTripPlanDto>> GetAllTripPlanAsync();
        Task CreateTripPlanAsync(CreateTripPlanDto createTripPlanDto);
        Task UpdateTripPlanAsync(UpdateTripPlanDto updateTripPlanDto);
        Task DeleteTripPlanAsync(string id);
        Task<GetTripPlanByIdDto> GetTripPlanByIdAsync(string id);
    }
}
