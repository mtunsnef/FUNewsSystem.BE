using AutoMapper;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.NewsArticleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.AutoMapper
{
    public class NewsArticleProfile : Profile
    {
        public NewsArticleProfile()
        {
            CreateMap<CreateUpdateNewsArticleDto, NewsArticle>();
            CreateMap<NewsArticle, CreateUpdateNewsArticleDto>();
        }
    }
}
