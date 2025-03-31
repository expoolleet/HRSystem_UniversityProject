using Domain.Companies;

namespace Domain.Candidates;

public sealed class CandidateDocument
{
    public string Name { get; private init; }
    public string? Portfolio { get; private init; }
    public int WorkExperience { get; private init; }
    
    private CandidateDocument(User user, int workExperience, string? portfolio)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        if (workExperience < 0)
        {
            throw new Exception("Wrong work experience (negative value)");
        }

        Name = user.Name;
        WorkExperience = workExperience;
        Portfolio = portfolio;
    }

    public static CandidateDocument Create(
        User user,
        int workExperience,
        string? portfolio
    ) => new CandidateDocument(user, workExperience, portfolio);
}
