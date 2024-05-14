using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.SignalRModels;
using CleanArchitecture.Infrastructure.SignalR.Interfaces;

namespace CleanArchitecture.Infrastructure.SignalR.Services;

public class PlayerDataService :IPlayerDataService
{
    public async Task<PlayerDataModel> JoinPlayer(string name ,string form, string cId)
    {

        PlayerDataModel playerDataModel = new PlayerDataModel {Name = name, SubmittedForm = form, ConnectionId = cId};
        return playerDataModel; 

    }

    public Result RemovePlayer(string cId)
    {
        throw new NotImplementedException();
    }

    public PlayerDataModel SubmittedUserResult(QuestionDataModel question,  PlayerDataModel player , string answer ,int reponseTime)
    {

        var score=0;
       SubmittedQuestionAndAnswers questionAndAnswers = new SubmittedQuestionAndAnswers();
        var trueAnswer = question.ChoiceDataModels.FirstOrDefault(x => x.IsTrue);
        if (trueAnswer != null)
        {
            if (trueAnswer.ChoiceText == answer)
            {
               score= CalculateScore(question.Score, reponseTime , question.Duration);

               questionAndAnswers.IsCorrect = true;
               player.CurrentTotalScore += score;
               player.IsCurrentQuestionCorrect = true;

            }
            else
            {
                player.IsCurrentQuestionCorrect = false;

            }
        }

        questionAndAnswers.Id = question.Id;
        questionAndAnswers.QuestionTitle=question.Title;
        questionAndAnswers.CorrectChoice = trueAnswer.ChoiceText; 
        questionAndAnswers.SelectedChoice=answer;
        questionAndAnswers.Choices = question.ChoiceDataModels;
        questionAndAnswers.QuestionScore = score;
        
        player.SubmittedQuestionAndAnswersList.Add(questionAndAnswers);


        return player; 
    }

    private int CalculateScore(Score score ,int responseTime, int duration)
    {
         
        decimal response = decimal.Divide(duration- responseTime , duration);

        decimal divition = response / 2;

        decimal suptraction = 1 - divition;

        decimal muitipliction = (int)score * suptraction;
  
        return (int)muitipliction; 
    }
}

