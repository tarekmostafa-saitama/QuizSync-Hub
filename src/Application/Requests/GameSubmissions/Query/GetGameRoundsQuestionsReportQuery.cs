using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using CleanArchitecture.Application.Requests.Questions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetGameRoundsQuestionsReportQuery : IRequest<QuestionSubmitReportVm>
{
    public int GameId { get; set; }

    public QuestionVm Question { get; set; }
}

public class GetGameRoundsQuestionsReportQueryHandler : IRequestHandler<GetGameRoundsQuestionsReportQuery, QuestionSubmitReportVm>
{

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetGameRoundsQuestionsReportQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<QuestionSubmitReportVm> Handle(GetGameRoundsQuestionsReportQuery request, CancellationToken cancellationToken)
    {
        QuestionSubmitReportVm questionSubmitReport = new();
        var submits = await _dbContext.GameSubmissions.AsNoTracking().Where(x => x.GameId == request.GameId).ToListAsync(cancellationToken);


        questionSubmitReport.TotalSubmits = submits.Count();


        var submittedQuestions = submits.SelectMany(x => x.SubmittedQuestionAndAnswersList).Where(x => x.Id == request.Question.Id);
        request.Question.Choices.ForEach(x => questionSubmitReport.ChoicesReportData.Add(x.ChoiceText, 0));

        questionSubmitReport.ChoicesReportData.Add("TimeOut", 0);

        foreach (var submit in submittedQuestions)
        {
            if (submit.SelectedChoice == null)
                questionSubmitReport.ChoicesReportData["TimeOut"] += 1;


            else if (questionSubmitReport.ChoicesReportData.ContainsKey(submit.SelectedChoice))
                questionSubmitReport.ChoicesReportData[submit.SelectedChoice] += 1;

        }
        questionSubmitReport.QuestionTitle = request.Question.Title;
        return questionSubmitReport;
    }

}
