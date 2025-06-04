using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.DTOs.Request.AuthDto
{
    public class RefreshRequestDto
    {
        public string Token { get; set; } = default!;
    }
}
