﻿using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Domain.Entities;

public class GameSubmission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalScore { get; set; }
    public string SubmittedForm { get; set; }
    public DateTime DateTime { get; set; }

    public int SubmissionIdentifier { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
    public List<SubmittedQuestionAndAnswers> SubmittedQuestionAndAnswersList { get; set; }



}