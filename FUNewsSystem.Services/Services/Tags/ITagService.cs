using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.TagDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.Tags
{
    public interface ITagService
    {
        IQueryable<Tag> GetAll();
        Tag GetById(int id);
        Task<ApiResponseDto<string>> CreateAsync(CreateUpdateTagDto dto);
        Task<ApiResponseDto<string>> UpdateAsync(int id, CreateUpdateTagDto dto);
        Task<ApiResponseDto<string>> DeleteAsync(int id);
    }
}
