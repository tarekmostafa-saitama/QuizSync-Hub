using CleanArchitecture.Domain.SignalRModels;
using CleanArchitecture.Infrastructure.SignalR.Interfaces;

namespace CleanArchitecture.Infrastructure.SignalR.Services;

public class QuestionDataService : IQuestionDataService
{
    public QuestionDataModel GetNextQuestion(int index)
    {
        var questionData = new QuestionDataModel();
        return questionData; 
    }
}