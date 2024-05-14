using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Requests.Games.Models;

public class GameVm
{
    public GameVm()
    {
        Questions = new(); 
    }
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public int Code { get; set; }
    [Required]
    public bool IsVrEnabled { get; set; }

    //to do dynamic Form
    public string FormContent { get; set; }


    public string ModeratorId { get; set; }
    public ApplicationUser Moderator { get; set; }

    public List<Question> Questions { get; set; }


    public ThemeConfigurationVm ThemeConfiguration { get; set; }
}