using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.DTOs.Request.AuthDto
{
    public class LogoutRequestDto
    {
        public string Token { get; set; } = default!;
    }
}
