using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Domain.Entities;

public class Game
{

    public int Id { get; set; }
    public string  Name { get; set; }
    public int  Code { get; set; }
    public bool  IsVrEnabled { get; set; }

    //to do dynamic Form
    public string FormContent { get; set; }


    public string ModeratorId { get; set; }
    public ApplicationUser Moderator { get; set; }

    public List<Question> Questions { get; set; }
    public List<GameSubmission> Submissions { get; set; }


    public ThemeConfiguration ThemeConfiguration { get; set; }



}