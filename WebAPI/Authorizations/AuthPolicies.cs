namespace WebApi.Authorizations;

public static class AuthPolicies
{
    public const string RequireAdminRole = "RequireAdminRole";
    public const string RequireManagerRole = "RequireManagerRole";
    public const string RequireAdminOrManagerRole = "RequireAdminOrManagerRole";
    public const string RequireRecruiterRole = "RequireRecruiterRole";
}