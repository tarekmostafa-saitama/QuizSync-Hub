using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Requests.Questions.Models;

public class QuestionVm
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]

    public Score Score { get; set; }
    [Required]

    public int Duration { get; set; }
    public string PhotoUrl { get; set; }
    public IFormFile Photo { get; set; }
    public List<Choice> Choices { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }

}