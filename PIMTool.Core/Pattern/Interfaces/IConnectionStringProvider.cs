namespace PIMTool.Core.Pattern.Interfaces;

public interface IConnectionStringProvider
{
    string GetConnectionString(string connetion = "DefaultConnection");
}
