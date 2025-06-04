using AutoMapper;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.TagDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.AutoMapper
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<CreateUpdateTagDto, Tag>();
            CreateMap<Tag, CreateUpdateTagDto>();
        }
    }
}
