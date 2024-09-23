using Newtonsoft.Json;
using PIMTool.Client.Constants;
using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;
using PIMTool.Client.Repositories.Interfaces;
using Serilog;
using System.Net.Http;

namespace PIMTool.Client.Repositories.Implementations;

public class GroupRepository : IGroupRepository
{
    private readonly HttpClient _httpClient;

    public GroupRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ApiClients.PIMTool.Name);
    }

    public async Task<IEnumerable<Group>> GetAllGroups()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Groups);
            var rawResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<IEnumerable<Group>>>(rawResponse);

            return apiResponse?.Data ?? [];
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return [];
        }
    }
}
