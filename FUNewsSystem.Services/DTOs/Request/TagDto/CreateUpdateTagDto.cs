using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.DTOs.Request.TagDto
{
    public class CreateUpdateTagDto
    {
        public string? TagName { get; set; }
        public string? Note { get; set; }
    }
}
