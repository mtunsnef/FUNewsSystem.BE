using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.Services.NewsArticles;
using FUNewsSystem.Services.Services.Tags;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsSystem.API.Controllers
{
    public class TagODataController : ODataController
    {
        private readonly ITagService _tagService;

        public TagODataController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var result = _tagService.GetAll();
            return Ok(result);
        }

        [EnableQuery]
        public SingleResult<Tag> Get([FromODataUri] int key)
        {
            var result = _tagService.GetAll().Where(b => b.TagId == key);
            return SingleResult.Create(result);
        }
    }
}
