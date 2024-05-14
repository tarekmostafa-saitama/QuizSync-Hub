using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.ViewModels;

public class ChangePasswordVm
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Current Password is required.")]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New Password is required.")]
    [Display(Name = "New Password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm New Password is required.")]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "New  Password and Confirm New Password don't match")]
    public string ConfirmPassword { get; set; }
}