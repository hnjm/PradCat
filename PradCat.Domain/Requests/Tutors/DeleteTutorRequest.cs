using System.Text.Json.Serialization;

namespace PradCat.Domain.Requests.Tutors;
public class DeleteTutorRequest : Request
{
    [JsonIgnore]
    public int Id { get; set; }
}
