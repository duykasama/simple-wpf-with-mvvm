namespace PIMTool.Client.Constants;

public static class ApiClients
{
    public static readonly ApiClient PIMTool = new ApiClient("PIMToolAPI", "http://localhost:10000");
}

public class ApiClient
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;

    public ApiClient(string name, string address)
    {
        Name = name;
        Address = address;
    }
}
