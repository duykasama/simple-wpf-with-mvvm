using PIMTool.Core.Enums;

namespace PIMTool.Api.Requests;

public class SearchWithPaginationRequest : PaginationRequest
{
    public string Keyword { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; }
}
