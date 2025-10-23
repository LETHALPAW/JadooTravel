using JadooTravel.Dtos.ReservationDtos;
using JadooTravel.Dtos.TripPlanDtos;

namespace JadooTravel.Models
{
    public class TripPlanReservationViewModel
    {
        public List<ResultTripPlanDto> TripPlans { get; set; }
        public CreateReservationDto Reservation { get; set; }
    }
}
