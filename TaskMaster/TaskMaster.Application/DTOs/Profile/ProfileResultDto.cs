

namespace TaskMaster.Application.DTOs.Profile
{
    public class ProfileResultDto
    {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string FullName => $"{FirstName} {LastName}";
            public string? Email { get; set; } = string.Empty;
            public string? Avatar { get; set; }
        }
}
