namespace PradCat.Domain.Results;

public class CreateUserResult
{
    public bool Succeeded { get; set; }
    public string? UserId { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
