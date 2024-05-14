using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    

    [HttpGet("Admin")]
    public IActionResult Index()
    {
        return View();
    }
}