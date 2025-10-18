using System.Diagnostics;
using GymManagementBLL.Services.Interfaces;
using GymManagementPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticalService _analyticalService;

        public HomeController(IAnalyticalService analyticalService)
        {
            _analyticalService = analyticalService;
        }
        public IActionResult Index()
        {
            var analytics = _analyticalService.GetAnalyticsData();
            
            return View(analytics);
        }
    }
}
