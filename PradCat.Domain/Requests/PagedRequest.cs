namespace PradCat.Domain.Requests;
public abstract class PagedRequest : Request
{
    public int PageSize { get; set; } = Configuration.DEFAULT_PAGE_SIZE;
    public int PageNumber { get; set; } = Configuration.DEFAULT_PAGE_NUMBER;
}
