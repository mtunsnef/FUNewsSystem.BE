using FUNewsSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.DTOs.Request.AuthDto
{
    public class LoginPayloadDto
    {
        public TokenPayloadDto AccessToken { get; set; }
        public bool? Authenticated { get; set; }
    }

}
