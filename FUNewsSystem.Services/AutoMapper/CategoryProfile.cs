using AutoMapper;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.CategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetAllCategoryDto>();
            CreateMap<CreateUpdateCategoryDto, Category>();
            CreateMap<Category, CreateUpdateCategoryDto>();
        }
    }
}
