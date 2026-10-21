using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{
    public record LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public record RegisterDTO : LoginDto
    {
        public string UserName { get; set; }
        public String DisplayName { get; set; }
    }

    public record ResetPasswordDTO : LoginDto
    {
        public string Token { get; set; }
    }

    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}