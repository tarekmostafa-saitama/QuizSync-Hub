using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class RearrangeQuestionCommand: IRequest<bool>
{
    public int GameId { get; set; }
    public List<RearrangeQuestionsVm> Model { get; set; }

    public RearrangeQuestionCommand(List<RearrangeQuestionsVm> model, int gameId)
    {
        Model = model;
        GameId = gameId;
    }
}

public class RearrangeQuestionCommandHandler : IRequestHandler<RearrangeQuestionCommand, bool>
{

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public RearrangeQuestionCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RearrangeQuestionCommand request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Questions.Where(x=> x.GameId == request.GameId).ToListAsync(cancellationToken);
        if (items == null)
            throw new Exception("Error...");


        foreach (var question in items)
        {
            foreach (var model in request.Model)
            {
                if (question.Id == model.Id)
                {
                    question.Order= model.Order;

                }
            }
        }


        _dbContext.Questions.UpdateRange(items);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;


    }
}
