using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.CategoryDto;
using FUNewsSystem.Services.DTOs.Response;

namespace FUNewsSystem.Services.Services.Categories
{
    public interface ICategoryService
    {
        Category GetById(short id);
        Task<ApiResponseDto<string>> CreateAsync(CreateUpdateCategoryDto dto);
        Task<ApiResponseDto<string>> UpdateAsync(short id, CreateUpdateCategoryDto dto);
        Task<ApiResponseDto<string>> DeleteAsync(short id);
        IQueryable<Category> GetAll();
    }
}
