namespace PradCat.Domain.Responses;
public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }

    public Response(T? data, string message, bool success)
    {
        Data = data;
        Message = message;
        Success = success;
    }

    // mensagem padrão para retorno de sucesso
    public static Response<T> SuccessResponse(T? data, string message = "Request successful")
        => new(data, message, true);

    // mensagem personalizada de erro
    public static Response<T> ErrorResponse(string message)
        => new(default, message, false);
}
