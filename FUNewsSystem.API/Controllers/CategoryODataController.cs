using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.Services.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsSystem.API.Controllers
{
    public class CategoryODataController : ODataController
    {
        private readonly ICategoryService _categoryService;

        public CategoryODataController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var result = _categoryService.GetAll();
            return Ok(result);
        }

        [EnableQuery]
        public SingleResult<Category> Get([FromODataUri] int key)
        {
            var result = _categoryService.GetAll().Where(b => b.CategoryId == key);
            return SingleResult.Create(result);
        }
    }
}