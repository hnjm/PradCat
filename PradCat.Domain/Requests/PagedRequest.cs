using System.Text.Json.Serialization;

namespace PradCat.Domain.Requests;
public abstract class PagedRequest
{
    public int PageSize { get; set; } = Configuration.DEFAULT_PAGE_SIZE;
    public int PageNumber { get; set; } = Configuration.DEFAULT_PAGE_NUMBER;
}
