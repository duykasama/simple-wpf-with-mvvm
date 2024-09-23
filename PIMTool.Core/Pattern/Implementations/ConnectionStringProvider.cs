using Microsoft.Extensions.Configuration;
using PIMTool.Core.Pattern.Interfaces;

namespace PIMTool.Core.Pattern.Implementations;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString(string connection = "DefaultConnection")
    {
        var connectionString = _configuration.GetConnectionString(connection);
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

        return connectionString;
    }
}
