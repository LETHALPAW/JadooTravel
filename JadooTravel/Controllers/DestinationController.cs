using JadooTravel.Dtos.DestinationDtos;
using JadooTravel.Services.DestinationServices;
using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;

namespace JadooTravel.Controllers
{

    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly LanguageService _languageService;
       
        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
            
        }

        public async Task<IActionResult> DestinationList()
        {
            var values = await _destinationService.GetAllDestinationAsync();
            return View(values);
        }

        public async Task<IActionResult> GetDestinationById(string id)
        {
            var value = await _destinationService.GetDestinationByIdAsync(id);
            return View(value);
        }

        [HttpGet]
        public IActionResult CreateDestination()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDestination(CreateDestinationDto createDestinationDto)
        {
            await _destinationService.CreateDestinationAsync(createDestinationDto);
            return RedirectToAction("DestinationList");
        }

        public async Task<IActionResult> DeleteDestination(string id)
        {
            await _destinationService.DeleteDestinationAsync(id);
            return RedirectToAction("DestinationList");

        }

        [HttpGet]
        public async Task<IActionResult> UpdateDestination(string id)
        {
            var value = await _destinationService.GetDestinationByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDestination(UpdateDestinationDto updateDestinationDto)
        {
            await _destinationService.UpdateDestinationAsync(updateDestinationDto);
            return RedirectToAction("DestinationList");
        }

    }
}
