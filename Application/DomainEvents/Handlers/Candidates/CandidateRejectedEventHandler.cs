using Application.Candidates.Repositories;
using Application.Mail;
using Domain.Events.Candidates;
using MediatR;

namespace Application.DomainEvents.Handlers.Candidates;

public class CandidateRejectedEventHandler : INotificationHandler<DomainEventNotification<CandidateRejectedEvent>>
{
    private readonly IMailService _mailService;
    private readonly ICandidateRepository _candidateRepository;

    public CandidateRejectedEventHandler(IMailService mailService, ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(mailService);
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _mailService = mailService;
        _candidateRepository = candidateRepository;
    }
    public async Task Handle(DomainEventNotification<CandidateRejectedEvent> notification, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.Get(notification.DomainEvent.CandidateId, cancellationToken);
        
        var mailData = new MailData
        {
            EmailToId = candidate.Document.Email,
            EmailToName = candidate.Document.Name,
            EmailSubject = "Your Candidate Information",
            EmailBody = $"Bad news!, {candidate.Document.Name}!\nYour candidature is rejected!\n\nregards, HR Team.",
        };
        await _mailService.SendMail(mailData);
    }
}