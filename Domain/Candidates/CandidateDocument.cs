
namespace Domain.Candidates;

public sealed class CandidateDocument
{
    public string Name { get; private init; }
    public string Portfolio { get; private init; }
    public int WorkExperience { get; private init; }
    
    public string Email { get; private init; }
    
    private CandidateDocument(string name, int workExperience, string portfolio, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (workExperience < 0)
        {
            throw new Exception("Wrong work experience (negative value)");
        }

        Name = name;
        WorkExperience = workExperience;
        Portfolio = portfolio;
        Email = email;
    }
    
    private CandidateDocument() { }

    public static CandidateDocument Create(
        string name,
        int workExperience,
        string portfolio,
        string email
    ) => new CandidateDocument(name, workExperience, portfolio, email);
}
