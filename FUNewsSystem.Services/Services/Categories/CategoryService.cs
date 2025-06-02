using AutoMapper;
using AutoMapper.QueryableExtensions;
using FUNewsSystem.Domain.Exceptions.Http;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.Categories;
using FUNewsSystem.Services.DTOs.CategoryDto;
using FUNewsSystem.Services.DTOs.Response;
using System.Net;

namespace FUNewsSystem.Services.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public IQueryable<Category> GetAll()
        {
            var query = _categoryRepository.GetAll();
            return query;
        }

        public async Task<ApiResponseDto<string>> CreateAsync(CreateUpdateCategoryDto dto)
        {
            var exists = _categoryRepository.GetAll().Any(c => c.CategoryName == dto.CategoryName);
            if (exists)
                throw new ConflictException("Category with the same name already exists.");

            var category = _mapper.Map<Category>(dto);
            _categoryRepository.Add(category);
            await _categoryRepository.SaveAsync();
            return ApiResponseDto<string>.Success("Category created successfully.");
        }

        public async Task<ApiResponseDto<string>> UpdateAsync(short id, CreateUpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with Id {id} not found.");

            _mapper.Map(dto, category);
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();

            return ApiResponseDto<string>.Success("Category updated successfully.");
        }

        public async Task<ApiResponseDto<GetAllCategoryDto>> GetByIdAsync(short id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with Id {id} not found.");

            var dto = _mapper.Map<GetAllCategoryDto>(category);
            return ApiResponseDto<GetAllCategoryDto>.Success(dto);
        }
        public async Task<ApiResponseDto<string>> DeleteAsync(short id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with Id {id} not found.");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();

            return ApiResponseDto<string>.Success("Category deleted successfully.");
        }

    }
}
