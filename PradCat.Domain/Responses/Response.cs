namespace PradCat.Domain.Responses;
public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public Response(T? data, string message, int statusCode)
    {
        Data = data;
        Message = message;
        StatusCode = statusCode;
    }

    // mensagem padrão para retorno de sucesso
    public static Response<T> SuccessResponse(T? data, string message = "Request successful", int statusCode = 200)
        => new(data, message, statusCode);

    // mensagem personalizada de erro
    public static Response<T> ErrorResponse(string message, int statusCode = 400)
        => new(default, message, statusCode);
}
