using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CleanArchitecture.Application.ViewModels;

public class AlterUserPasswordViewModel
{

    public string UserId { get; set; }

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