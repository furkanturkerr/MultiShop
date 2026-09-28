using MultiShop.Dtos.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class CommentService : ICommentService
{
    private readonly HttpClient _client;

    public CommentService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultCommentDto>> GetAllCommentAsync()
    {
        var response = await _client.GetAsync("comment/Comments");
        if (!response.IsSuccessStatusCode)
            return new List<ResultCommentDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultCommentDto>>() ?? new List<ResultCommentDto>();
    }

    public async Task<List<ResultCommentDto>> GetCommentByProductIdAsync(string productId)
    {
        var address = $"comment/Comments/CommentByProductId?productId={Uri.EscapeDataString(productId)}";
        var response = await _client.GetAsync(address);
        if (!response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return new List<ResultCommentDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultCommentDto>>() ?? new List<ResultCommentDto>();
    }

    public async Task<UpdateCommentDto?> GetByIdCommentAsync(int id)
    {
        var response = await _client.GetAsync($"comment/Comments/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateCommentDto>();
    }

    public async Task<bool> CreateCommentAsync(CreateCommentDto dto)
    {
        var response = await _client.PostAsJsonAsync("comment/Comments", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCommentAsync(UpdateCommentDto dto)
    {
        var response = await _client.PutAsJsonAsync("comment/Comments", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await _client.DeleteAsync($"comment/Comments?id={id}");
        response.EnsureSuccessStatusCode();
    }
}
