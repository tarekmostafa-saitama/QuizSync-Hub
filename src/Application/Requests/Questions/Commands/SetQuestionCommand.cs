using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Helpers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Questions.Commands;

public class SetQuestionCommand : IRequest<int>
{
    public SetQuestionCommand(int gameId , QuestionVm question)
    {
        GameId = gameId;
        Question = question;
    }

    public QuestionVm Question { get; set; }
    public int GameId { get; set; }
}

public class SetQuestionCommandHandler : IRequestHandler<SetQuestionCommand, int>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public SetQuestionCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ISender sender)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<int> Handle(SetQuestionCommand request, CancellationToken cancellationToken)
    {
        if (request.Question.Photo != null)
        {
            if (!string.IsNullOrEmpty(request.Question.PhotoUrl))
            {
                FileManager.Delete(request.Question.PhotoUrl, "uploads");
            }

            request.Question.PhotoUrl =
                await FileManager.Upload(request.Question.Photo, "uploads", false);
        }
        var model = _mapper.Map<Question>(request.Question);

        if (model.Id == default)
        {

            Question newQuestion = new Question
            {
                Title = model.Title,
                Duration = model.Duration, 
                Score = model.Score, 
                PhotoUrl = model.PhotoUrl, 
                Choices = model.Choices,
                GameId = model.GameId
             
            };

            _dbContext.Questions.Add(newQuestion);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newQuestion.Id;
        }

        var question = await _dbContext.Questions.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==model.Id, cancellationToken: cancellationToken);

        if (question == null) throw new NotFoundException("No Question Found" , model.Id);

        
        _dbContext.Questions.Update(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return question.Id;
    

    }


}