using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Questions.Commands;

public class DeleteQuestionCommand : IRequest<bool>
{
    public DeleteQuestionCommand(int questionId)
    {
        QuestionId = questionId;
    }

    public int QuestionId { get; set; }

}

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteQuestionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    
    }
    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.Questions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken);
        if (model == null)
            return false;
        _dbContext.Questions.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}


