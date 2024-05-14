using System.Linq.Expressions;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.SignalRModels;
using CleanArchitecture.Infrastructure.SignalR.Interfaces;
using MediatR;

namespace CleanArchitecture.Infrastructure.SignalR.Services;

public class GameDataService :IGameDataService
{
    private readonly ISender _sender;

    public GameDataService(ISender sender)
    {
        _sender = sender;
    }
    public async Task<GameDataModel> GetGameDataByCode(int code , string connectionId)
    {
        var game = await _sender.Send(new GetGameByCodeQuery(code,
            new List<Expression<Func<Game, object>>>() {x => x.Questions, y => y.ThemeConfiguration}));
        var gameData = new GameDataModel
        {
            Id = game.Id,
           Code = game.Code,
           IsVrEnabled= game.IsVrEnabled,
           ModeratorId = game.ModeratorId, 
           ModeratorConnectionId = connectionId, 
           Name = game.Name, 
           ThemeConfigurationModel = new ThemeConfigurationModel(),          
           Questions = new List<QuestionDataModel>(), 
           
        };
        if (game.ThemeConfiguration != null)
        {
            gameData.ThemeConfigurationModel.BackgroundImageUrl = game.ThemeConfiguration.BackgroundImageUrl;
            gameData.ThemeConfigurationModel.HeaderLogoUrl= game.ThemeConfiguration.HeaderLogoUrl;
            gameData.ThemeConfigurationModel.NavbarThemeColor = game.ThemeConfiguration.SecondaryThemeColor;
            gameData.ThemeConfigurationModel.MainThemeColor = game.ThemeConfiguration.MainThemeColor;
        }
        if (game.Questions != null)
        {
            foreach (var question in game.Questions.OrderBy(x=>x.Order))
            {
                QuestionDataModel model = new QuestionDataModel
                {
                    Id = question.Id,
                    Title = question.Title,
                    Score = question.Score,
                    PhotoUrl = question.PhotoUrl,
                    Duration = question.Duration,
                };

                foreach (var questionChoice in question.Choices)
                {
                    ChoiceDataModel choice = new ChoiceDataModel { ChoiceText = questionChoice.ChoiceText, IsTrue = questionChoice.IsTrue, };
                    model.ChoiceDataModels.Add(choice);
                }
                gameData.Questions.Add(model);
            }

        }


        return gameData;
    }
}