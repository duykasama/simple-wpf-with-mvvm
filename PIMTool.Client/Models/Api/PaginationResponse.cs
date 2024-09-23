namespace PIMTool.Client.Models.Api;

public class PaginationResponse<T>
{
    public ICollection<T> Items { get; set; } = [];

    public int PageSize { get; set; }

    public int PageIndex { get; set; }

    public int LastPage { get; set; }

    public bool IsLastPage { get; set; }

    public int Total { get; set; }
}
