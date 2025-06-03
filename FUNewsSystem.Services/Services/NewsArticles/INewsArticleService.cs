using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.NewsArticleDto;
using FUNewsSystem.Services.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.NewsArticles
{
    public interface INewsArticleService
    {
        NewsArticle GetById(string id);
        Task<ApiResponseDto<string>> CreateNewsArticle(CreateUpdateNewsArticleDto dto);
        Task<ApiResponseDto<string>> UpdateNewsArticle(string id, CreateUpdateNewsArticleDto dto);
        Task<ApiResponseDto<string>> DeleteAsync(string id);
        IQueryable<NewsArticle> GetAll();
    }
}
