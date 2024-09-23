using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Requests;

public class PaginationRequest
{
    [Range(1, 50)]
    public int PageSize { get; set; }

    [Range(1, int.MaxValue)]
    public int PageIndex { get; set; }
};
