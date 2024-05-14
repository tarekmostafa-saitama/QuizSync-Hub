using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Moderator.Controllers;

    [Area("Moderator")]
    [Authorize(Roles = "Moderator")]
    public class HomeController : Controller
    {




        [HttpGet("Moderator")]

        public IActionResult Index()
        {
            return View();
        }
    }
