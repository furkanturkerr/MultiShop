using MultiShop.Catalog.Dtos.CategoryDtos;

namespace MultiShop.Catalog.Services.CategoryServices;

public interface ICategoryService
{
    Task<List<ResultCategoryDto>> GetAllCategoriesAsync();

    Task CreateCategoryAsync(CreateCategoryDto dto);

    Task UpdateCategoryAsync(UpdateCategoryDto dto);

    Task<GetByIdCategoryDto> GetCategoryByIdAsync(string id);

    Task DeleteCategoryByIdAsync(string id);
}