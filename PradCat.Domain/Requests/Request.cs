using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests;
public abstract class Request
{
    public string UserId { get; set; } = string.Empty;
}
