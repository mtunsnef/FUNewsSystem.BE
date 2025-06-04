using AutoMapper;
using FUNewsSystem.Domain.Exceptions.Http;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.Tags;
using FUNewsSystem.Services.DTOs.Request.TagDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.Tags
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public IQueryable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public Tag GetById(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag == null)
                throw new NotFoundException($"Tag with Id {id} not found.");

            return tag;
        }

        public async Task<ApiResponseDto<string>> CreateAsync(CreateUpdateTagDto dto)
        {
            var exists = _tagRepository.GetAll().Any(t => t.TagName == dto.TagName);
            if (exists)
                throw new ConflictException("Tag with the same name already exists.");

            var tag = _mapper.Map<Tag>(dto);
            _tagRepository.Add(tag);
            await _tagRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Tag created successfully.");
        }

        public async Task<ApiResponseDto<string>> UpdateAsync(int id, CreateUpdateTagDto dto)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                throw new NotFoundException($"Tag with Id {id} not found.");

            _mapper.Map(dto, tag);
            _tagRepository.Update(tag);
            await _tagRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Tag updated successfully.");
        }

        public async Task<ApiResponseDto<string>> DeleteAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
                throw new NotFoundException($"Tag with Id {id} not found.");

            _tagRepository.Delete(tag);
            await _tagRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("Tag deleted successfully.");
        }
    }
}
