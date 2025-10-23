using JadooTravel.Dtos.ReservationDtos;
using JadooTravel.Models;
using JadooTravel.Services.ReservationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{

    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _reservationService.GetAllReservationAsync();
            return View(values);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateReservation(TripPlanReservationViewModel model)
        {
            if (model.Reservation != null)
            {
                model.Reservation.Created = DateTime.Now;
                await _reservationService.CreateReservationAsync(model.Reservation);
                TempData["ReservationSuccess"] = "Rezervasyon başarıyla alındı!";
            }

            return RedirectToAction("Index", "Default");
        }


        public async Task<IActionResult> DeleteReservation(string id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return RedirectToAction("Index", "Reservation");


        }
    }
}

