namespace PIMTool.Client.Models;

public class SearchCriteria
{
    public string Keyword { get; set; } = string.Empty;
    public ProjectStatus? Status { get; set; }
}
