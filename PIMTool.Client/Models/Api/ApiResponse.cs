namespace PIMTool.Client.Models.Api;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }

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

    public ApiResponse(bool isSuccess, T data)
    {
        IsSuccess = isSuccess;
        Data = data;
    }

    public ApiResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}
