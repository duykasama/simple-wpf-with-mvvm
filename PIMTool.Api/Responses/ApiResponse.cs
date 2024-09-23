namespace PIMTool.Api.Responses;

public class ApiResponse<T>
{
    public string? Title { get; set; }

    public T? Data { get; set; }

    public ICollection<string> Messages { get; set; } = [];

    public ApiResponse()
    {
    }

    public ApiResponse(T data)
    {
        Data = data;
    }
}
