namespace PradCat.Domain.Responses;
public class PagedResponse<T> : Response<T>
{
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = Configuration.DEFAULT_PAGE_SIZE;
    public int CurrentPage { get; set; }


    public PagedResponse(
        T? data,
        string message,
        int statusCode,
        int totalCount,
        int totalPages,
        int pageSize,
        int currentPage)
        : base(data, message, statusCode)
    {
        TotalCount = totalCount;
        TotalPages = totalPages;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public static PagedResponse<T> SuccessPagedResponse(T? data, int totalCount, int pageSize, int currentPage, string message = "Request successful", int statusCode = 200)
    {
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResponse<T>(data, message, statusCode, totalCount, totalPages, pageSize, currentPage);
    }

    public static PagedResponse<T> ErrorPagedResponse(string message, int statusCode = 400)
        => new PagedResponse<T>(default, message, statusCode, 0, 0, 0, 0);
}
