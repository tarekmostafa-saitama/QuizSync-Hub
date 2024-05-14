using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using NuGet.Protocol.Plugins;

namespace WebUI.Controllers;
public class AccountController : Controller
{
    // GET
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IToastNotification _toastNotification;
    private readonly UserManager<ApplicationUser> _userManager;


    public AccountController(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
        _toastNotification = toastNotification;
    }

    [HttpGet("~/")]
    [HttpGet("Account/Login")]
    public async Task<IActionResult> Login()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                _toastNotification.AddSuccessToastMessage("Logged in successfully");

                var isAdmin = await _userManager.IsInRoleAsync(user, Role.Admin.ToString());

                return isAdmin ? RedirectToAction("Index", "Home", new { Area = "Admin" })
                    : RedirectToAction("Index", "Home", new { Area = "Moderator" });

            }
        }

        return View();
    }
    [Route("Account/Login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
           

            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                _toastNotification.AddSuccessToastMessage("Logged in successfully");
                var isAdmin = await _userManager.IsInRoleAsync(user, Role.Admin.ToString());

                return isAdmin ? RedirectToAction("Index" , "Home" , new { Area = "Admin" }) 
                    : RedirectToAction("Index", "Home" , new { Area = "Moderator"});



            }

            ModelState.AddModelError("", "Invalid Login Attempt.");
        }

        return View(model);
    }

    [Authorize]
    [HttpPost("Account/ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm passViewModel , string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePasswordAsync(user,
                passViewModel.CurrentPassword, passViewModel.NewPassword);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage("Password Changed successfully");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                var isAdmin = await _userManager.IsInRoleAsync(user, Role.Admin.ToString());

                return isAdmin ? RedirectToAction("Index", "Home", new { Area = "Admin" })
                    : RedirectToAction("Index", "Home", new { Area = "Moderator" });



            }

            ModelState.AddModelError("", $"{result.Errors.First().Description}");
        }
        else
        {
            var error = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).First();

            ModelState.AddModelError("", $"{error}");
        }


        return View(passViewModel);

    }

    [Route($"Account/ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string? returnUrl = null)
    {
       

        return View();
    }



    [HttpGet("Account/RedirectToAccount")]
    public async Task<IActionResult> RedirectToAccount(string? email)
    {

        var user = await _userManager.FindByEmailAsync((email ?? User.Identity?.Name) ?? string.Empty);

        if (user == null)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        if (await _userManager.IsInRoleAsync(user, Role.Admin.ToString()))
            return RedirectToAction("Index", "Home", new { area = "Admin" });

        throw new Exception("");
    }

    [Route("Account/Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _toastNotification.AddSuccessToastMessage("Logged Out successfully");

        return RedirectToAction(nameof(Login));
    }


   
}
