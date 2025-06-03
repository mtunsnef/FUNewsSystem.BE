using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.Services.Categories;
using FUNewsSystem.Services.Services.NewsArticles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsSystem.API.Controllers
{
    public class NewsArticleODataController : ODataController
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticleODataController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var result = _newsArticleService.GetAll();
            return Ok(result);
        }

        [EnableQuery]
        public SingleResult<NewsArticle> Get([FromODataUri] string key)
        {
            var result = _newsArticleService.GetAll().Where(b => b.NewsArticleId.Equals(key));
            return SingleResult.Create(result);
        }
    }
}
