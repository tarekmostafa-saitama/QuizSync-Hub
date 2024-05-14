using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Score Score { get; set; }
    public int Duration { get; set; }
    public string PhotoUrl { get; set; }
    public int Order { get; set; }

    public List<Choice> Choices { get; set; }


    public int GameId { get; set; }
    public Game Game { get; set; }
}