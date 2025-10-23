using JadooTravel.Dtos.ReservationDtos;

namespace JadooTravel.Services.ReservationServices
{
    public interface IReservationService
    {
        Task<List<ResultReservationDto>> GetAllReservationAsync();
        Task CreateReservationAsync(CreateReservationDto createReservationDto);
        Task DeleteReservationAsync(string id);
          
    }
}
