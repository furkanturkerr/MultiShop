using MultiShop.Dtos.CatalogDtos.CategoryDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface ICategoryService
{
    Task<List<ResultCategoryDto>> GetAllCategoryAsync();
    Task<UpdateCategoryDto?> GetByIdCategoryAsync(string id);
    Task<bool> CreateCategoryAsync(CreateCategoryDto dto);
    Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto);
    Task DeleteCategoryAsync(string id);
}
