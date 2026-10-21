using Ecom.core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDto login);
        Task<bool> SendEmailForForgetPasword(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPassword);

        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);

    }

}
