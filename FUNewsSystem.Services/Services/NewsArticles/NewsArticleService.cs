using AutoMapper;
using FUNewsSystem.Domain.Exceptions.Http;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.NewsArticles;
using FUNewsSystem.Services.DTOs.Request.NewsArticleDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.NewsArticles
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IMapper _mapper;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
        }

        public IQueryable<NewsArticle> GetAll()
        {
            return _newsArticleRepository.GetAll();
        }

        public NewsArticle GetById(string id)
        {
            var article = _newsArticleRepository.GetById(id);
            if (article == null)
                throw new NotFoundException($"NewsArticle with Id {id} not found.");

            return article;
        }

        public async Task<ApiResponseDto<string>> CreateNewsArticle(CreateUpdateNewsArticleDto dto)
        {
            var article = _mapper.Map<NewsArticle>(dto);

            _newsArticleRepository.Add(article);
            await _newsArticleRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("NewsArticle created successfully.");
        }

        public async Task<ApiResponseDto<string>> UpdateNewsArticle(string id, CreateUpdateNewsArticleDto dto)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
                throw new NotFoundException($"NewsArticle with Id {id} not found.");

            _mapper.Map(dto, article);
            _newsArticleRepository.Update(article);
            await _newsArticleRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("NewsArticle updated successfully.");
        }

        public async Task<ApiResponseDto<string>> DeleteAsync(string id)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
                throw new NotFoundException($"NewsArticle with Id {id} not found.");

            _newsArticleRepository.Delete(article);
            await _newsArticleRepository.SaveAsync();

            return ApiResponseDto<string>.SuccessResponse("NewsArticle deleted successfully.");
        }
    }

}
