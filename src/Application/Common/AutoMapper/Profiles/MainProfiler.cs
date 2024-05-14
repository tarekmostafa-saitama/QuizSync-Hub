using AutoMapper;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Domain.Entities;


namespace CleanArchitecture.Application.Common.AutoMapper.Profiles;

public class MainProfiler : Profile
{
    public MainProfiler()
    {
        CreateMap<Game, GameVm>().ReverseMap();
        CreateMap<GameSubmission, GameSubmissionVm>().ReverseMap();
        CreateMap<ThemeConfiguration, ThemeConfigurationVm>().ReverseMap();

        CreateMap<Question, QuestionVm>().ReverseMap();
        CreateMap<GameSubmission, GameSubmissionVm>().ReverseMap();

    }
}