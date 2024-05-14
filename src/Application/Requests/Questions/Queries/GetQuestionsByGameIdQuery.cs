using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Questions.Queries;

public class GetQuestionsByGameIdQuery :IRequest<List<QuestionVm>>
{
    public GetQuestionsByGameIdQuery(int gameId , List<Expression<Func<Question, object>>> includes = null)
    {
        GameId = gameId;
        Includes = includes ?? new List<Expression<Func<Question, object>>>();

    }

    public int GameId { get; set; }
    public List<Expression<Func<Question, object>>> Includes { get; set; }
}

public class GetQuestionsByGameIdQueryHandler : IRequestHandler<GetQuestionsByGameIdQuery, List<QuestionVm>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetQuestionsByGameIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<QuestionVm>> Handle(GetQuestionsByGameIdQuery request, CancellationToken cancellationToken)
    {
        var model = _dbContext.Questions.AsNoTracking();
        foreach (var include in request.Includes)
        {
            model = model.Include(include);
        }

        return _mapper.Map<List<QuestionVm>>(await model.Where(x => x.GameId == request.GameId).ToListAsync(cancellationToken));
    }
}