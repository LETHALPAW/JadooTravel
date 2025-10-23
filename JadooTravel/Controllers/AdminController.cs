using JadooTravel.Services.DestinationServices;
using JadooTravel.Services.ReservationServices;
using JadooTravel.Services.TestimonialServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly IReservationService _reservationService;

        public AdminController(IDestinationService destinationService, IReservationService reservationService)
        {
            _destinationService = destinationService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            // 🗺️ Destination verileri (grafik için)
            var destinations = await _destinationService.GetAllDestinationAsync();
            ViewBag.DestinationNames = destinations.Select(x => x.CityCountry).ToList();
            ViewBag.Capacities = destinations.Select(x => x.Capacity).ToList();

            
            var reservations = await _reservationService.GetAllReservationAsync();
            ViewBag.Reservations = reservations;

            var last4Tours = destinations
                .OrderByDescending(x => x.DestinationId)
                .Take(4)
                .ToList();
            ViewBag.LastTours = last4Tours;
            ViewBag.TotalReservations = 236;         
            ViewBag.ThisMonthReservations = 42;     
            ViewBag.GrowthRate = 12;                 
            ViewBag.WebsiteRate = 50;             
            ViewBag.InstagramRate = 30;
            ViewBag.WhatsappRate = 20;

            return View();
        }
    }
}
