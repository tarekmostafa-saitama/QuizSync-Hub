using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
using CleanArchitecture.Application.Requests.Questions.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Questions.Queries;

public class GetQuestionQuery : IRequest<QuestionVm>
{
    public int QuestionId { get; set; }
    public List<Expression<Func<Question, object>>> Includes { get; set; }

    public GetQuestionQuery(int questionId, List<Expression<Func<Question, object>>> includes = null)
    {
        Includes = includes ?? new List<Expression<Func<Question, object>>>();
        QuestionId = questionId;
    }
}
public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionVm>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetQuestionQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<QuestionVm> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var model = _dbContext.Questions.AsNoTracking();
        foreach (var include in request.Includes)
        {
            model = model.Include(include);
        }

        return _mapper.Map<QuestionVm>(await model.FirstOrDefaultAsync(x => x.Id == request.QuestionId,
            cancellationToken));
    }

}