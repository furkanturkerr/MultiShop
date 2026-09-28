using MultiShop.Dtos.CommentDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface ICommentService
{
    Task<List<ResultCommentDto>> GetAllCommentAsync();
    Task<List<ResultCommentDto>> GetCommentByProductIdAsync(string productId);
    Task<UpdateCommentDto?> GetByIdCommentAsync(int id);
    Task<bool> CreateCommentAsync(CreateCommentDto dto);
    Task<bool> UpdateCommentAsync(UpdateCommentDto dto);
    Task DeleteCommentAsync(int id);
}
