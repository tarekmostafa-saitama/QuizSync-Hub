using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Infrastructure.SignalR.Interfaces;

public interface IQuestionDataService
{
    QuestionDataModel  GetNextQuestion(int index);

}