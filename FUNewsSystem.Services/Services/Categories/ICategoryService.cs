using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.CategoryDto;
using FUNewsSystem.Services.DTOs.Response;

namespace FUNewsSystem.Services.Services.Categories
{
    public interface ICategoryService
    {
        Task<ApiResponseDto<GetAllCategoryDto>> GetByIdAsync(short id);
        Task<ApiResponseDto<string>> CreateAsync(CreateUpdateCategoryDto dto);
        Task<ApiResponseDto<string>> UpdateAsync(short id, CreateUpdateCategoryDto dto);
        Task<ApiResponseDto<string>> DeleteAsync(short id);
        IQueryable<Category> GetAll();
    }
}
