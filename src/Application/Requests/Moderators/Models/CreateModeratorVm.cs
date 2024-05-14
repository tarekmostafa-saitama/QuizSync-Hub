using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Requests.Moderators.Models;

public class CreateModeratorVm
{
    [Required(ErrorMessage = "Please Enter Full Name")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Please Enter Email")]

    public string Email { get; set; }
    [Required(ErrorMessage = "Please Enter Password")]

    public string Password { get; set; }
}
