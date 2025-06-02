using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.CategoryDto;
using FUNewsSystem.Services.Services.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NuGet.Protocol.Core.Types;

namespace FUNewsSystem.API.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [EnableQuery] // Cho phép filter, select, expand, etc.
        [HttpGet]
        public IActionResult Get()
        {
            var result = _categoryService.GetAll();
            return Ok(result);
        }


        //[EnableQuery]
        //[HttpGet("{key}")]
        //public async Task<IActionResult> Get([FromODataUri] short key)
        //{
        //    var result = await _categoryService.GetByIdAsync(key);
        //    return Ok(result);
        //}

        //public async Task<IActionResult> Post([FromBody] CreateUpdateCategoryDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var response = await _categoryService.CreateAsync(dto);
        //    return Created($"odata/Categories({response.Data})", response);
        //}

        //public async Task<IActionResult> Put([FromODataUri] short id, [FromBody] CreateUpdateCategoryDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var response = await _categoryService.UpdateAsync(id, dto);
        //    return Ok(response);
        //}

        //public async Task<IActionResult> Delete([FromODataUri] short id)
        //{
        //    var response = await _categoryService.DeleteAsync(id);
        //    return Ok(response);
        //}
    }

}
