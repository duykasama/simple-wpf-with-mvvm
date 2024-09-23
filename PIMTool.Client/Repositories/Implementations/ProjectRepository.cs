using Newtonsoft.Json;
using PIMTool.Client.Constants;
using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;
using PIMTool.Client.Repositories.Interfaces;
using Serilog;
using System.Net.Http;
using System.Net.Http.Json;

namespace PIMTool.Client.Repositories.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly HttpClient _httpClient;

    public ProjectRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ApiClients.PIMTool.Name);
    }

    public async Task<ApiResponse<object>> CreateProject(CreateProjectRequest project)
    {
        try
        {
            var payload = JsonContent.Create(project);
            var response = await _httpClient.PostAsync(ApiRoutes.Projects, payload);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<object>(isSuccess: true);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(responseContent);

            return new ApiResponse<object>(isSuccess: false, data: error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return new ApiResponse<object>(isSuccess: false, data: null!);
        }
    }

    public async Task<bool> DeleteMultipleProjects(IList<int> selectedProjectIds)
    {
        try
        {
            var payload = JsonContent.Create(selectedProjectIds);
            var request = new HttpRequestMessage(HttpMethod.Delete, ApiRoutes.ProjectsMany)
            {
                Content = payload
            };
            var response = await _httpClient.SendAsync(request);

            var deleteSuccess = response.IsSuccessStatusCode;
            if (!deleteSuccess)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Log.Error(responseContent);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteProjectById(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(string.Format(ApiRoutes.ProjectsSingle, id));
            var deleteSuccess = response.IsSuccessStatusCode;
            if (!deleteSuccess)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Log.Error(responseContent);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return false;
        }
    }

    public async Task<ApiResponse<object?>> GetProjectById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(string.Format(ApiRoutes.ProjectsSingle, id));

            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                var finalResponse = JsonConvert.DeserializeObject<ApiResponse<Project>>(dataAsString);

                return new ApiResponse<object?>(isSuccess: false, data: finalResponse);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(responseContent);
            return new ApiResponse<object?>(isSuccess: false, data: error);
        }
        catch (JsonSerializationException ex)
        {
            Log.Error($"Can not serialize response from server: \"{ex.Message}\"");
            Log.Error(ex, ex.Message);
            return new ApiResponse<object?>(isSuccess: false, data: null);
        }
    }

    public async Task<ApiResponse<PaginationResponse<Project>>> GetProjects(int pageSize, int pageIndex)
    {
        try
        {
            var queryParams = $"pageSize={pageSize}&pageIndex={pageIndex}";
            var response = await _httpClient.GetAsync($"{ApiRoutes.Projects}?{queryParams}");
            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                var finalResponse = JsonConvert.DeserializeObject<ApiResponse<PaginationResponse<Project>>>(dataAsString);
                if (finalResponse == null)
                {
                    return new ApiResponse<PaginationResponse<Project>>(false);
                }

                finalResponse.IsSuccess = true;
                return finalResponse;
            }

            return new ApiResponse<PaginationResponse<Project>>(false);
        }
        catch (JsonSerializationException ex)
        {
            Log.Error($"Can not serialize response from server: \"{ex.Message}\"");
            Log.Error(ex, ex.Message);
            return new ApiResponse<PaginationResponse<Project>>(false);
        }
    }

    public async Task<ApiResponse<PaginationResponse<Project>>> SearchProjects(string keyword, int? status, int pageSize, int pageIndex)
    {
        try
        {
            var queryParams = $"pageSize={pageSize}&pageIndex={pageIndex}";
            if (!string.IsNullOrEmpty(keyword))
            {
                queryParams += $"&keyword={keyword}";
            }
            if (status != null)
            {
                queryParams += $"&status={status}";
            }

            var response = await _httpClient.GetAsync($"{ApiRoutes.ProjectsSearch}?{queryParams}");
            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                var finalResponse = JsonConvert.DeserializeObject<ApiResponse<PaginationResponse<Project>>>(dataAsString);
                if (finalResponse == null)
                {
                    return new ApiResponse<PaginationResponse<Project>>(false);
                }

                finalResponse.IsSuccess = true;
                return finalResponse;
            }

            return new ApiResponse<PaginationResponse<Project>>(false);
        }
        catch (JsonSerializationException ex)
        {
            Log.Error($"Can not serialize response from server: \"{ex.Message}\"");
            Log.Error(ex, ex.Message);
            return new ApiResponse<PaginationResponse<Project>>(false);
        }
    }

    public async Task<ApiResponse<object>> UpdateProject(UpdateProjectRequest project)
    {
        try
        {
            project.Visas = project.Visas ?? [];
            var payload = JsonContent.Create(project);
            var response = await _httpClient.PutAsync(string.Format(ApiRoutes.ProjectsSingle, project.Id), payload);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<object>(isSuccess: true);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(responseContent);

            return new ApiResponse<object>(isSuccess: false, data: error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return new ApiResponse<object>(isSuccess: false, data: null!);
        }
    }
}
