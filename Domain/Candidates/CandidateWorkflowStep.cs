using Domain.Companies;

namespace Domain.Candidates;

public sealed class CandidateWorkflowStep
{
    private CandidateWorkflowStep(
        Guid? userId,
        Guid? roleId,
        int number,
        CandidateStatus status,
        string? feedback,
        DateTime? feedbackDate)
    {
        if (userId is null && roleId is null)
        {
            ArgumentNullException.ThrowIfNull(roleId);
        }

        UserId = userId;
        RoleId = roleId;
        Number = number;
        Status = status;
        Feedback = feedback;
        FeedbackDate = feedbackDate;
    }

    public Guid? UserId { get; private init; }
    public Guid? RoleId { get; private init; }
    public int Number { get; private init; }
    public CandidateStatus Status { get; private set; }
    public string? Feedback { get; private set; }
    public DateTime? FeedbackDate { get; private set; }

    public static CandidateWorkflowStep Create(Guid? userId, Guid? roleId, int number)
        => new(
            userId,
            roleId,
            number,
            CandidateStatus.InProcessing,
            feedback: null,
            feedbackDate: null);


    internal void Approve(User user, string feedback)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(feedback);

        CanChangeStatus(user);

        Status = CandidateStatus.Approved;
        Feedback = feedback;
        FeedbackDate = DateTime.UtcNow;
    }

    internal void Reject(User user, string feedback)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(feedback);

        Status = CandidateStatus.Rejected;
        Feedback = feedback;
        FeedbackDate = DateTime.UtcNow;
    }

    internal void Restart()
    {
        Status = CandidateStatus.InProcessing;
        Feedback = null;
        FeedbackDate = null;
    }

    private void CanChangeStatus(User user)
    {
        if (Status != CandidateStatus.InProcessing)
        {
            throw new Exception("Can not change status due: status is permanent (Approved/Rejected)");
        }

        if (user.Id != UserId && user.RoleId != RoleId)
        {
            throw new Exception("Can not change status due: wrong [UserId] and(or) [RoleId]");
        }
    }
}
