
using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            var result = await _work.Auth.RegisterAsync(registerDTO);

            if (!string.Equals(result, "done", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ResponseAPI(400, result ?? "Invalid request"));
            }

            return Ok(new ResponseAPI(200, "Registered successfully, please check your email"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            var result = await _work.Auth.LoginAsync(loginDto);

            if (string.IsNullOrEmpty(result) ||
                result.StartsWith("please", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ResponseAPI(400, result ?? "Invalid request"));
            }

            // CHANGED: شلنا Domain، وخلينا SameSite=None مع Secure عشان الكوكي تتخزن من Angular
            Response.Cookies.Append("token", result, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            // CHANGED: رجّعنا JSON بدل نص الـ JWT الخام (ده كان سبب فشل الـ parse في Angular)
            return Ok(new ResponseAPI(200, "Logged in successfully"));
        }

        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount([FromBody] ActiveAccountDTO accountDTO)
        {
            var result = await _work.Auth.ActiveAccount(accountDTO);
            return result
                ? Ok(new ResponseAPI(200, "Account activated successfully"))
                : BadRequest(new ResponseAPI(400, "Invalid token or email"));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> SendEmailForForgetPassword([FromQuery] string email)
        {
            var result = await _work.Auth.SendEmailForForgetPasword(email);
            return result
                ? Ok(new ResponseAPI(200, "Email sent successfully"))
                : BadRequest(new ResponseAPI(400, "Failed to send email"));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _work.Auth.ResetPassword(resetPasswordDTO);

            if (!string.Equals(result, "done", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ResponseAPI(400, result ?? "Invalid request"));
            }

            return Ok(new ResponseAPI(200, "Password reset successfully"));
        }
    }
}