using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.ViewModels;

public class LoginViewModel
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Display(Name = "Remember me ?")] public bool RememberMe { get; set; }
}